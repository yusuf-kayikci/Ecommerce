/* eslint-disable */
//import * as jQuery from 'jquery';
//import { awef } from './awef';
//import { awe } from './awe';
//import { awem, aweui } from './all';
//import { awedict } from './awedict'

var utils = function ($) {
    var loop = awef.loop, isNull = awef.isNull, isNotNull = awef.isNotNull;

    function toLowerFirst(s) {
        return s.substr(0, 1).toLowerCase() + s.substr(1);
    }

    function getColVal(str, o) {
        var v = getColsVal(str, o);
        return v ? v.join(' ') : '';
    }

    function getColsVal(name, o) {
        var names = name.split(',');
        return $.map(names, function (c) { return getNestVal(c.split('.'), o); });
    }

    function getNestVal(trail, o) {
        var val = o[trail.shift()];

        if (!trail.length) {
            if (val instanceof Date) {
                val = awem.formatDate('dd/mm/yy', val);
            }

            return val;
        }

        if (!val) return 0;
        return getNestVal(trail, val);
    }

    function id(o) {
        return o.Id || o.id;
    }

    function content(o) {
        return o.Content || o.content;
    }

    function flashz(d, back) {
        d.addClass('awe-ts');
        if (back)
            d.removeClass('awe-hl');
        else
            d.addClass('awe-hl');

        setTimeout(function () {
            d.removeClass('awe-ts');
        }, 500);
    }

    function setLrsObsl(g) {
        g.data('o').lrso = 1;
    }

    function dapi(g) {
        return g.data('api');
    }

    var flash = awe.flash;

    var Sort = {
        None: 0,
        Asc: 1,
        Desc: 2
    };

    var NodeType =
    {
        Node: 1,
        Items: 2,
        Lazy: 3
    };

    function getPage(items, p, ps) {
        var skip = (p - 1) * ps;
        return items.slice(skip, skip + ps);
    }

    function closeNest(nest) {
        dapi(nest.closest('.awe-grid')).nestClose(nest, 1);
    }

    function itemCreated(gridId) {
        return function (item) {
            var g = $("#" + gridId);
            var row = $(dapi(g).renderRow(item));

            g.trigger('awerowch', 1);
            g.find(".awe-content .awe-tbody").prepend(row);
            flash(row);

            var data = g.data('o').lrs.dt;
            if (data.it) {
                data.it.unshift(item);
            } else {
                setLrsObsl(g);
            }
        };
    }

    function itemEdited(gridId, nof, noTrigEdit) {
        return function (item) {
            var $grid = $('#' + gridId);
            var gapi = dapi($grid);
            var xhr = gapi.update(id(item));

            var row = gapi.select(id(item))[0];
            row.nextUntil(':not(.awe-nest)').each(function () {
                closeNest($(this));
            });

            $.when(xhr).done(function () {
                var newRow = gapi.select(id(item))[0];
                flash(newRow);

                setLrsObsl($grid);
                if (!nof) {
                    newRow.find('.editbtn').focus();
                }

                if (!noTrigEdit) {
                    newRow.trigger('itemedit');
                }
            });
        };
    }

    function handlePopVld(data, popup) {
        var errs = data._errs;
        if (errs && Object.keys(errs).length) {
            for (var name in errs) {
                var vmsg = '';
                loop(errs[name], function (msg) {
                    vmsg += '<div class="field-validation-error">' + msg + '</div>';
                });

                popup.find('[vld-for="' + name + '"]').html(vmsg);
            }

            return true;
        }
    }

    return {
        imgCaption: function (o) {
            return '<img class="cthumb" src="' + o.url + '" />' + o.c;
        },
        getColVal: getColVal,
        // grid crud
        delConfirmLoad: function (gridId) {
            return function () {

                if (!gridId) return;
                var $popup = this.p.d;

                var key = $popup.find('[name="id"]').val();
                var rows = dapi($('#' + gridId)).select(key);

                $.map(rows, function (row) {
                    flashz(row);
                });

                $popup.on('aweclose', function () {
                    $.map(rows, function (row) {
                        flashz(row, 1);
                    });
                });
            };
        },

        itemDeleted: function (gridId) {
            return function (res) {
                var $grid = $("#" + gridId);
                var row = dapi($grid).select(id(res))[0];
                row.find('button').attr('disabled', 'disabled');
                row.trigger('itemdelete');
                utils.delRow(row);
            };
        },

        delRow: function (row) {
            var g = row.closest('.awe-grid');
            row.nextUntil(':not(.awe-nest)').each(function () {
                closeNest($(this));
            });

            var next = row.next();
            if (!next.length || next.hasClass('awe-ghead')) {
                row.prevUntil('.awe-row').fadeOut(500);
            }

            row.fadeOut(500, function () {
                $(this).remove();
                if (!row.is('tr') || !g.find('.awe-row').length) { //|| g.data('o').gl 
                    dapi(g).load();
                    return;
                }

                g.trigger('awerowch', -1);
                setLrsObsl(g);
            });
        },
        initPopupInstVld: function (o, vldf) {
            var form = o.scon.find('form');
            utils.initInstVld(form, vldf);
        },
        initInstVld: function (cont, vldf) {
            cont.on('change', function (e) {
                var trg = $(e.target);
                var name = trg.attr('name');

                if (awef.isNull(name)) return;
                var sd = cont.find(':input').serializeArray();

                var ms = vldf(sd);
                var errors = ms.errors;
                cont.find('[vld-for]').each(function () {
                    var el = $(this);
                    var fname = el.attr('vld-for');
                    if (awef.isNotNull(errors[fname])) {
                        if (name === fname) {
                            var vmsg = '';
                            awef.loop(errors[fname],
                                function (msg) {
                                    vmsg += '<div class="field-validation-error">' + msg + '</div>';
                                });
                            el.html(vmsg);
                        }
                    } else {
                        el.html('');
                    }
                });
            });
        },

        itemEditedUi: function (gridId, nof, noTrigEdit) {
            return function (data, popup) {
                if (!handlePopVld(data, popup)) {
                    itemEdited(gridId, nof, noTrigEdit)(data);
                    popup.data('api').close();
                }
            }
        },

        itemCreatedUi: function (gridId) {
            return function (data, popup) {
                if (!handlePopVld(data, popup)) {
                    itemCreated(gridId)(data);
                    popup.data('api').close();
                }
            }
        },

        itemEdited: itemEdited,

        itemCreated: itemCreated,

        // grid nest
        loadNestPopup: function (popupName) {
            return function (row, nestrow, cell) {
                var params = {};
                params['id'] = id(utils.model($(row)));
                awe.open(popupName, { params: params, tag: { cont: cell } });
                cell.one('aweclose', function (e) {
                    if ($(e.target).is(cell.find('.awe-popup:first'))) {
                        closeNest(nestrow);
                    }
                });
            };
        },

        nestCreate: function (gridId, popup) {
            var $grid = $('#' + gridId).addClass('o-nstcreate');
            var place = $grid.find('.awe-content:first');
            awe.open(popup, { tag: { cont: place } });
            $grid.one('aweclose', function () {
                $grid.removeClass('o-nstcreate');
            });
        },

        // ajaxlist crud
        itemCreatedAlTbl: function (listId) {
            return function (o) {
                var row = $(content(o));
                $('#' + listId).parent().find('tbody').prepend(row);
                flash(row);
            };
        },

        itemEditedAl: function (listId, func) {
            return function (o) {
                var $item = $('#' + listId).find('[data-val="' + id(o) + '"]');
                var $newItem = $(content(o));
                $item.after($newItem).remove();

                flash($newItem, func);
            };
        },

        itemDeletedAl: function (listId) {
            return function (o) {
                $('#' + listId).find('[data-val="' + id(o) + '"]').fadeOut(500, function () { $(this).remove(); });
            };
        },

        itemCreatedAl: function (listId) {
            return function (o) {
                var it = $($.trim(content(o)));
                $('#' + listId).prepend(it);
                flash(it);
            };
        },

        scheduler: function (id, popupName) {
            var $grid = $('#' + id);
            var $sched = $grid.closest('.scheduler');
            var api = dapi($grid);
            var $viewType = $sched.find('.viewType .awe-val');

            $sched.find('.prevbtn').click(function () {
                api.load({ oparams: { cmd: 'prev' } });
            });

            $sched.find('.nextbtn').click(function () {
                api.load({ oparams: { cmd: 'next' } });
            });

            $sched.find('.todaybtn').click(function () {
                api.load({ oparams: { cmd: 'today' } });
            });

            $grid
                .on('aweload', function (e, data) {
                    var tag = data.tg;

                    if (tag.View === 'Agenda' || tag.View === 'Month') {
                        $('.schedBotBar').hide();
                    }
                    else
                        $('.schedBotBar').show();

                    if ($viewType.val() !== tag.View) {
                        dapi($viewType.val(tag.View)).render();
                    }

                    $sched.find('.schDate .awe-val').val(tag.Date);
                    $sched.find('.dateLabel').html(tag.DateLabel);
                })
                .on('click', '.eventTitle', function () {
                    awe.open('edit' + popupName, { params: { id: $(this).parent().data('id') } });
                })
                .on('click', '.delEvent', function () {
                    awe.open('delete' + popupName, { params: { id: $(this).closest('.schEvent').data('id') } });
                })
                .on('dblclick', 'td', function (e) {
                    var schdev = $(e.target).closest('.schEvent');
                    if (!schdev.length) {
                        var tp = $(this).find('.timePart');
                        awe.open('create' + popupName,
                            { params: { ticks: tp.data('ticks'), allDay: tp.data('allday') } });
                    } else {
                        if (!$(e.target).is('.delEvent'))
                            awe.open('edit' + popupName, { params: { id: schdev.data('id') } });
                    }
                })
                .on('click', '.day', function (e) {
                    if ($(e.target).is('.day')) {
                        api.load({ oparams: { viewType: 'Day', date: $(this).data('date') } });
                    }
                });
        },

        // misc
        refreshGrid: function (gridId) {
            return function () {
                dapi($("#" + gridId)).load();
            };
        },

        getMinutesOffset: function () {
            return { minutesOffset: new Date().getTimezoneOffset() };
        },

        // used for .DataFunc, items is KeyContent[]
        getItems: function (items) {
            return function () {
                return items;
            };
        },

        getEmpty: function () {
            return [];
        },

        escapeChars: function (str) {
            var entityMap = {
                "&": "&amp;",
                '"': '&quot;',
                "'": "&#39;"
            };
            return String(str).replace(/[&"'\/]/g, function (s) {
                return entityMap[s];
            });
        },

        serializeObj: function (a, arrays, singles) {
            var res = {};
            arrays = arrays || [];

            loop(a, function (it) {
                var val = it.value;
                var name = it.name;

                if (isNull(val)) val = '';

                if (isNotNull(res[name])) {
                    if ($.inArray(name, singles) < 0) {
                        if (!res[name].push) {
                            res[name] = [res[name]];
                        }

                        res[name].push(val);
                    }
                } else {
                    res[name] = $.inArray(name, arrays) >= 0 ? [val] : val;
                }
            });

            return res;
        },

        getParams: function(a, arrays, singles) {
            var res = {};
            arrays = arrays || [];

            loop(a, function (it) {
                var val = it.value;
                var name = it.name ? toLowerFirst(it.name) : '';

                if (isNull(val)) val = '';

                if (isNotNull(res[name])) {
                    if ($.inArray(name, singles) < 0) {
                        if (!res[name].push) {
                            res[name] = [res[name]];
                        }

                        res[name].push(val);
                    }
                } else {
                    res[name] = $.inArray(name, arrays) >= 0 ? [val] : val;
                }
            });

            return res;
        },

        getGridParams: function (a) {
            return utils.serializeObj(a, ["SortNames", "SortDirections", "Groups", "Headers"], ["page", "pageSize", "Paging"]);
        },

        init: function (dateFormat, isMobileOrTablet, decimalSep) {
            // checkbox
            awe.chkmd = awem.ochk;

            // set tabs mod
            awe.tmd = [awem.tbtns];

            // set lookup mod
            awe.lmd = awem.lookupKeyNav;

            // set datepicker
            awe.dpw = awem.dtp;

            awe.acw = awem.autocomplete;

            // grid global mods
            awe.ggmd = function (o) {
                var md = o.md = o.md || [];
                var amp = awem.gridAutoMiniPager;

                // grid loading anim, without no records msg
                var gld = awem.gldng(0, 1);

                if (md.indexOf(amp) === -1 && md.indexOf(awem.gridMiniPager) === -1) {
                    md.push(amp);
                }

                if (md.indexOf(gld) === -1) {
                    md.unshift(gld);
                }
            };

            !isMobileOrTablet && awem.initWave();

            // add antiforgery token before request
            var aftoken;
            awe.bfr = function (opt) {
                if (opt.type !== 'post') return;
                var key = '__RequestVerificationToken';
                var data = opt.data || [];
                if (!aftoken) aftoken = $('[name="' + key + '"]').val();
                if (aftoken) {
                    if (data.push) {
                        var found = 0;
                        for (var i = 0; i < data.length; i++) {
                            if (data[i].name === key) found = 1;
                        }

                        if (!found) {
                            data.push({ name: key, value: aftoken });
                        }
                    } else if (data.append && data.has) {
                        if (!data.has(key)) {
                            data.append(key, aftoken);
                        }
                    }
                }

                opt.data = data;
            };

            if (dateFormat) {
                awem.dateFormat = dateFormat;
            }

            // by default jquery.validate doesn't validate hidden inputs
            if ($.validator) {
                $.validator.setDefaults({
                    ignore: [],
                    highlight: function (element, error) {
                        var $el = $(element);
                        if ($el.hasClass('awe-val')) {
                            var $fl = $el.closest(".awe-field");
                            if ($fl.length) {
                                $fl.addClass(error);
                            } else {
                                $el.addClass(error);
                            }
                        }
                    },
                    unhighlight: function (element, error) {
                        var $el = $(element);
                        if ($el.hasClass('awe-val')) {
                            var $fl = $el.closest(".awe-field");
                            if ($fl.length) {
                                $fl.removeClass(error);
                            } else {
                                $el.removeClass(error);
                            }
                        }
                    }
                });

                if (dateFormat) {
                    setjQueryValidateDateFormat(dateFormat);
                }

                $(function () {
                    // parsing the unobtrusive attributes when we get content via ajax
                    $(document).ajaxComplete(function () {
                        if ($.validator.unobtrusive) {
                            $.validator.unobtrusive.parse(document);
                        }
                    });
                });
            }

            if (decimalSep === ',') setjQueryValidateDecimalSepComm();

            utils.setPopup();

            var $doc = $(document);
            if (!$doc.data('aweldngset')) {
                $doc.data('aweldngset', 1);
                $(function () {
                    utils.consistentSearchTxt();
                    $(document).ajaxComplete(utils.consistentSearchTxt);
                    setFormLoadingAnim();
                });
            }

            // remove locaStorage keys from older versions of awesome; you can modify  ppk (awe.ppk = "myapp1_"), current value is "awe17_"
            try {
                for (var key in localStorage) {
                    if (key.indexOf("awe") === 0) {
                        if (key.indexOf(awe.ppk) !== 0) {
                            localStorage.removeItem(key);
                        }
                    }
                }
            } catch (err) {
                /*empty*/
            }

            /*begin*/
            awe.err = function (o, p2) {
                var msg = "unexpected error occured";
                if (p2) {
                    if (typeof p2 === 'string') {
                        msg = p2;
                    } else {
                        msg = p2.responseText || msg;
                    }
                }

                var content = msg;
                // handle ajax errors that return html with layout page
                if (msg.indexOf('<html>') > -1) {
                    content = $('<div>unexpected error<br/> (showing it will replace the whole page)<div/>');
                    var btn = $('<a href="">show</a>').click(function (e) {
                        e.preventDefault();
                        $('body').html(msg);
                    });

                    content.append(btn);
                }

                awem.notif(content, 0, 'o-err');

                // close popup that got error
                if (o && o.p && o.p.isOpen) {
                    dapi(o.p.d).close();
                }
            };
            /*end*/

            function setFormLoadingAnim() {

                function setLoading(cont, marg) {
                    var spin = $('<div class="spinCont"><div class="spinner"><div class="dot1"></div><div class="dot2"></div></div></div>').hide();
                    cont.append(spin);
                    var ldng = cont.find('.awe-loading:first');
                    cont.on('aweload', function () {
                        spin.remove();
                    });

                    setTimeout(function () {
                        if (cont.width() > 200 && cont.height() > 90) {
                            ldng.hide();
                            var h = cont.outerHeight(true);
                            spin.height(h);
                            spin.children().first().css('margin-top', h / 2 - marg + 'px');
                            spin.show();
                        }
                    }, 100);
                }

                $(document)
                    .on('awebeginload', function (e) {

                        var cont = $(e.target);

                        if (cont.is('.awe-popup')) {
                            setLoading(cont, 30);
                        }
                    })
                    .on('submit', function (e) {
                        var marg = 50;
                        var cont = $(e.target).closest('.awe-popupw, .formLoad');

                        if (cont.is('.formLoad')) {
                            marg = 30;
                        }

                        if (cont.length) {
                            setLoading(cont, marg);
                        }
                    });
            }

            function setjQueryValidateDateFormat(format) {
                //setting the correct date format for jquery.validate
                $.validator.addMethod(
                    'date',
                    function (value, element) {
                        if (this.optional(element)) {
                            return true;
                        }

                        var result = false;
                        try {
                            awem.parseDate(format, value);
                            result = true;
                        } catch (err2) {
                            result = false;
                        }
                        return result;
                    },
                    ''
                );
            }

            function setjQueryValidateDecimalSepComm() {
                if ($.validator) {
                    $.validator.methods.range = function (value, element, param) {
                        var globalizedValue = value.replace(",", ".");
                        return this.optional(element) || globalizedValue >= param[0] && globalizedValue <= param[1];
                    };

                    $.validator.methods.number = function (value, element) {
                        return this.optional(element) || /^-?(?:\d+|\d{1,3}(?:[\s\.,]\d{3})+)(?:[\.,]\d+)?$/.test(value);
                    };
                }
            }
        },

        remLSPref: function (pref, newName) {
            if (localStorage)
                for (var key in localStorage) {
                    if (key.indexOf(pref) === 0) {
                        if (key !== newName) {
                            localStorage.removeItem(key);
                        }
                    }
                }
        },

        // on ie hitting enter doesn't trigger change, 
        // all searchtxt inputs will trigger change on enter in all browsers
        consistentSearchTxt: function () {
            $('.searchtxt').each(function () {
                if ($(this).data('searchtxth') !== 1)
                    $(this).data('searchtxth', 1)
                        .data('myval', $(this).val())
                        .on('change', function (e) {
                            if ($(this).val() !== $(this).data('myval')) {
                                $(this).data('myval', $(this).val());
                            } else {
                                e.stopImmediatePropagation();
                            }
                        })
                        .on('keyup', function (e) {
                            if (e.which === 13) {
                                e.preventDefault();
                                if ($(this).val() !== $(this).data('myval')) {

                                    $(this).change();
                                }
                            }
                        });
            });
        },
        setPopup: function () {
            awe.popup = function (o) {
                if (o.tag && o.tag.DropdownPopup) {
                    return awem.dropdownPopup(o);
                } else if (o.tag && o.tag.Inline) {
                    return awem.inlinePopup(o);
                } else {
                    return awem.dropdownPopup(o);
                }
            };
        },
        postfix: function (o) {
            return function (val) {
                return val ? val + ' ' + o : '';
            };
        },

        prefix: function (o) {
            return function (val) {
                return val ? o + val : '';
            };
        },
        percent: function (val) {
            return (parseFloat(val.replace(',', '.')) * 100).toFixed() + ' %';
        },

        gridModelBuilder: function (opt) {
            var gp = opt.gp;
            var items = opt.items;
            
            var itemsCount = opt.itemsCount;

            if (isNull(gp.Paging)) gp.Paging = 1;

            var key = opt.key;
            var defaultKeySort = opt.defaultKeySort;
            if (isNull(defaultKeySort)) defaultKeySort = Sort.Desc;

            var sortNames = gp.SortNames || [];
            var sortDirections = gp.SortDirections || [];
            var getChildren = opt.getChildren;
            var paging = isNotNull(gp.Paging) ? gp.Paging : 1;

            var page = gp.page || 1;
            var lazyKey = gp.key;
            var getItem = opt.getItem;
            var pageCount;

            calcOrderParams();

            var map = opt.map || function (it) { return it; };

            var makeFooter = opt.makeFooter || function () { return null; };

            var areInSameGroup = opt.areInSameGroup || function (path, g1, g2) {
                if (path.indexOf(",") === -1) return getColVal(path, g1) === getColVal(path, g2);
                var props = path.split(',');

                var result = 1;
                $.each(props, function (i, prop) {
                    result = result && getColVal(prop, g1) === getColVal(prop, g2);
                });

                return result;
            };

            var makeKey = opt.makeKey || function (gkey, groupIndex, group, val) {
                return gkey + "$" + groupIndex + group + encodeURIComponent(val); //make $0gkey$1gkey2
            };

            var makeHeader = opt.makeHeader || function (info) {
                if (info.NodeItem) {
                    return { Item: map(info.NodeItem) };
                }

                var val = isNotNull(info.Val) ? info.Val : getColVal(info.Column, info.Items[0]);

                return {
                    Content: info.Header + ": " + val,
                    Collapsed: 0
                };
            };

            // build model
            var treeHeight = 0;
            
            if (opt.getChildren && !opt.key) {
                throw new Error("gridModelBuilder key should have value when GetChildren is set");
            }

            if (isNull(items) && isNull(opt.pageItems) && isNull(gp.key))
            {
                throw new Error("GridModelBuilder items not set, either set the items, or set the pageItems proeperty");
            }
            
            var pageSize = parseInt(gp.pageSize || 10);
            if (pageSize < 1) pageSize = 10;

            var pageItems = items;

            if (!isNull(opt.pageItems)) {
                pageItems = opt.pageItems;
            } else {
                // when no custom querying
                if (isNull(itemsCount)) {
                    
                    if (!lazyKey) {
                        items = orderBy(items);
                        if (paging) {
                            pageItems = getPage(items, page, pageSize);

                            if (pageItems.length === 0 && page > 1) {
                                page = 1;
                                pageItems = getPage(items, page, pageSize);
                            }
                            
                        } else {
                            pageItems = items.slice(0);
                        }
                    } else {
                        // Lazy Key load
                        if (!getItem) {
                            throw new Error("GridModelBuilder getItem func needs to be defined (used by Lazy Loading and api.update)");
                        }
                        
                        var list = [];
                        var item = getItem();
                        if (item) list.push(item);
                        pageItems = list;
                    }
                }
            }

            gp.Groups = gp.Groups || [];

            var data = buildData(lazyKey);

            if (isNull(itemsCount))
            {
                itemsCount = items.length;
            }

            if (isNull(pageCount))
            {
                pageCount = Math.ceil(itemsCount / pageSize);
            }

            var model = {
                Data: data,
                PageCount: pageCount,
                ItemsCount: itemsCount,
                PageSize: pageSize,
                Page: paging ? page : -1,
                Pgn: gp.Paging,
                GroupCount: gp.Groups.length,
                Th: treeHeight,
                Key: key,
                Fr: opt.FrozenRows
            };

            function ModelToDto(input) {
                var res =
                {
                    k: input.Key,
                    th: input.Th,
                    p: input.Page,
                    cs: input.Cs,
                    fr: input.Fr,
                    gc: input.GroupCount,
                    ic: input.ItemsCount,
                    pc: input.PageCount,
                    pgn: input.Pgn,
                    ps: input.PageSize,
                    tg: input.Tag,
                    A: 1
                };

                if (isNotNull(input.Data)) {
                    res.dt = ToGroupViewDto(input.Data);
                }

                return res;
            }

            function ToGroupViewDto(o) {
                var res =
                {
                    it: o.Items,
                    f: o.Footer,
                    nt: o.Nt,
                    h: ToGHeaderDto(o.Header)
                };

                if (isNotNull(o.Groups)) {
                    res.gs = $.map(o.Groups, ToGroupViewDto);
                }

                return res;
            }

            function ToGHeaderDto(o) {
                if (isNull(o)) return null;

                var res =
                {
                    k: o.Key,
                    c: o.Content,
                    i: o.Item,
                    cl: o.Collapsed,
                    ip: o.IgnorePersistence
                };

                return res;
            }

            return ModelToDto(model);

            function calcOrderParams() {
                if (!sortNames.length && key && defaultKeySort !== Sort.None) {
                    // default sorting
                    sortNames = [key];
                    sortDirections = [defaultKeySort === Sort.Asc ? "asc" : "desc"];
                }
            }

            function orderBy(uitems) {
                if (sortNames.length) {
                    var getfunc = function (sname, a) {
                        return a[sname];
                    };
                    var getfuncs = $.map(sortNames, function (sname) {
                        if (sname.indexOf(',') + 1 || sname.indexOf('.') + 1)
                            return getColVal;
                        return getfunc;
                    });

                    uitems.sort(function (a, b) {
                        var res = 0;

                        loop(sortNames, function (sname, i) {
                            var direction = sortDirections[i];
                            var func = getfuncs[i];
                            var sa = func(sname, a), sb = func(sname, b);


                            if (typeof sa === "string") {
                                res = (sa || '').localeCompare(sb);
                            }
                            else {
                                res = sa - sb;
                            }

                            if (direction === 'desc') {
                                res = -res;
                            }

                            if (res !== 0) {
                                return false;
                            }
                        });

                        return res;
                    });
                }

                return uitems;
            }

            function buildData(lkey) {
                var gridModel = buildGroupView(pageItems, lkey ? [] : gp.Groups, gp.Headers, 0, "$" + page, !!lkey);

                return gridModel;
            }

            function buildGroupView(groupItems, groupColumns, groupNames, groupIndex, keyPart, keyHasVal) {
                function addGroupView() {
                    var first = groupViewItems[0];
                    var gcol = groupColumns[groupIndex];

                    var info =
                    {
                        Items: groupViewItems,
                        Column: gcol,
                        Header: groupNames[groupIndex],
                        Level: lvl
                    };

                    var val = getColVal(gcol, first);
                    if (opt.getHeaderVal) {
                        var colFunc = opt.getHeaderVal[gcol];
                        if (colFunc) {
                            val = colFunc(first);
                            info.Val = val;
                        }
                    }

                    var newKey = makeKey(keyPart, groupIndex, group, val);

                    var gr = buildGroupView(groupViewItems, groupColumns, groupNames, groupIndex + 1, newKey);
                    gr.Header = makeHeader(info);
                    gr.Header.Key = gr.Header.Key || newKey;
                    gr.Footer = makeFooter(info);

                    groupViews.push(gr);
                }

                var result = {};
                if (groupIndex == 0 && !keyHasVal) {
                    result.Footer = makeFooter({ Items: groupItems });
                }

                var lvl = groupIndex + 1;

                if (groupIndex === groupColumns.length) {
                    if (isNull(getChildren) || !groupItems.length) {
                        result.Items = $.map(groupItems, map);
                    }
                    else {
                        // set items or groups
                        buildNode(result, groupItems, key, lvl, 0);
                    }
                }
                else {
                    var groupViews = [];
                    var groupViewItems = [];

                    var group = groupColumns[groupIndex];
                    var i = 0;
                    while (i !== groupItems.length) {
                        var head = groupItems[i];

                        if (groupViewItems.length === 0)
                            groupViewItems.push(head);

                        else if (areInSameGroup(group, groupViewItems[0], head))
                            groupViewItems.push(head);

                        else {
                            addGroupView();
                            groupViewItems = [head];
                        }

                        i++;
                    }

                    if (groupViewItems.length !== 0) {
                        addGroupView();
                    }

                    result.Groups = groupViews;
                }

                return result;
            }

            function buildNode(result, groupItems, keyPart, level, nodeLevel) {
                if (nodeLevel > treeHeight) treeHeight = nodeLevel;

                var groups = [];

                $.each(groupItems, function (i, groupItem) {
                    var children = getChildren(groupItem, nodeLevel + 1) || [];
                    var isLazyNode = children === "lazy";

                    // if item has no children or lazy filter it, and ignore if filtered out

                    if (!isLazyNode && children.length > 1) {
                        //sort children
                        orderBy(children);
                    }

                    if (isLazyNode || children.length) {
                        var keyValue = getColVal(key, groupItem);
                        var newKey = makeNodeKey(keyValue);

                        var nodeGroup = {};
                        nodeGroup.Nt = isLazyNode ? NodeType.Lazy : NodeType.Node;
                        nodeGroup.Header = makeHeader({
                            Items: children,
                            NodeItem: groupItem,
                            Level: level,
                            NodeLevel: nodeLevel,
                            Lazy: isLazyNode
                        });

                        nodeGroup.Header.Key = nodeGroup.Header.Key || newKey;

                        if (isLazyNode) {
                            nodeGroup.Header.Collapsed = true;
                        }
                        else {
                            buildNode(nodeGroup, children, newKey, level, nodeLevel + 1);
                            // if returns true we don't filter it, otherwise we add it to the must be filtered collection
                        }

                        groups.push(nodeGroup);
                    }
                    else {
                        // leaf
                        if (nodeLevel > treeHeight) treeHeight = nodeLevel;

                        if (groups.length === 0 || groups[groups.length - 1].Nt !== NodeType.Items) {
                            groups.push({ Items: [map(groupItem)], Nt: NodeType.Items });
                        }
                        else {
                            var last = groups[groups.length - 1];
                            last.Items.push(map(groupItem));
                        }
                    }
                });

                if (groups.length === 1 && groups[0].Nt === NodeType.Items) {
                    result.Items = groups[0].Items;
                }
                else {
                    result.Groups = groups;
                }

                // filter 
                // return true if there's items after filtering
            }

            function makeNodeKey(val) {
                return "$" + encodeURIComponent(val);
            }
        },
        osearch: function (o, info) {
            var term = info.term;
            var c = info.cache;
            c.termsUsed = c.termsUsed || {};
            c.nrterms = c.nrterms || []; // no result terms

            if (c.termsUsed[term]) return [];
            c.termsUsed[term] = 1;

            // ignore terms that are contain nr terms
            var nores = 0;
            $.each(c.nrterms, function (i, val) {
                if (term.indexOf(val) >= 0) {
                    nores = 1;
                    return false;
                }
            });

            if (nores) {
                return [];
            }

            var prm = awe.params(o);
            prm.push({ name: "term", value: term });

            return awe.ajx({ url: o.tag.Url, data: prm }).then(function (data) {
                if (!data || !data.length) {
                    c.nrterms.push(term);
                }
                return data;
            }).fail(function () { c.termsUsed[term] = 0; });
        },
        model: function (row) {
            return dapi(row.closest('.awe-grid')).model(row) || {};
        },
        closeNest: closeNest,
        colf: function (cols) {
            return {
                fcoli: function (id) {
                    for (var i = 0; i < cols.length; i++) {
                        var col = cols[i];
                        if (col.I === id) return col;
                    }
                },
                fcol: function (bind) {
                    for (var i = 0; i < cols.length; i++) {
                        var col = cols[i];
                        if (col.P === bind) return col;
                    }
                }
            };
        },
        gcvw: function (api, col, opt) {
            return '<div class="awe-cv" data-i="' + col.ix + '">' + utils.gcv(api, col, opt) + '</div>';
        },
        gcv: function (api, col, opt) {
            return api.gcv(opt.itm, col, opt.nf);
        },
        getVisCols: function (g) {
            var columns = g.data('o').columns;
            var vcols = [];
            for (var i = 0; i < columns.length; i++) {
                var col = columns[i];
                if (!col.Hid) {
                    vcols.push(col.T || col.P);
                }
            }

            return vcols;
        },
        Sort: Sort,
        version: 6.3
    };
}(jQuery);

//export { utils };