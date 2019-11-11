using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Ecommerce.Models;

namespace Ecommerce.Core
{
    public class UnitOfWork : IUnitOfWork
    {
        //UOF

        private EcommerceContext _context;
        private BaseRepository<BillOfMaterials> _billOfMaterialsRepository;
        private BaseRepository<Culture> _cultureRepository;
        private BaseRepository<Illustration> _illustrationRepository;
        private BaseRepository<Location> _locationRepository;
        private BaseRepository<Product> _productRepository;
        private BaseRepository<ProductCategory> _productCategoryRepository;
        private BaseRepository<ProductCostHistory> _productCostHistoryRepository;
        private BaseRepository<ProductDescription> _productDescriptionRepository;
        private BaseRepository<ProductInventory> _productInventoryRepository;
        private BaseRepository<ProductListPriceHistory> _productListPriceHistoryRepository;
        private BaseRepository<ProductModel> _productModelRepository;
        private BaseRepository<ProductModelIllustration> _productModelIllustrationRepository;
        private BaseRepository<ProductModelProductDescriptionCulture> _productModelProductDescriptionCultureRepository;
        private BaseRepository<ProductPhoto> _productPhotoRepository;
        private BaseRepository<ProductReview> _productReviewRepository;
        private BaseRepository<ProductSubcategory> _productSubcategoryRepository;
        private BaseRepository<ScrapReason> _scrapReasonRepository;
        private BaseRepository<TransactionHistory> _transactionHistoryRepository;
        private BaseRepository<TransactionHistoryArchive> _transactionHistoryArchiveRepository;
        private BaseRepository<UnitMeasure> _unitMeasureRepository;
        private BaseRepository<WorkOrder> _workOrderRepository;
        private BaseRepository<WorkOrderRouting> _workOrderRoutingRepository;



        public UnitOfWork(EcommerceContext context)
        {
            _context = context;
        }
        public IRepository<BillOfMaterials> BillOfMaterialsRepository => _billOfMaterialsRepository ?? (_billOfMaterialsRepository = new BaseRepository<BillOfMaterials>(_context));
        public IRepository<Culture> CultureRepository => _cultureRepository ?? (_cultureRepository = new BaseRepository<Culture>(_context));
        public IRepository<Illustration> IllustrationRepository => _illustrationRepository ?? (_illustrationRepository = new BaseRepository<Illustration>(_context));
        public IRepository<Location> LocationRepository => _locationRepository ?? (_locationRepository = new BaseRepository<Location>(_context));
        public IRepository<Product> ProductRepository => _productRepository ?? (_productRepository = new BaseRepository<Product>(_context));
        public IRepository<ProductCategory> ProductCategoryRepository => _productCategoryRepository ?? ((_productCategoryRepository = new BaseRepository<ProductCategory>(_context)));
        public IRepository<ProductCostHistory> ProductCostHistoryRepository => _productCostHistoryRepository ?? ((_productCostHistoryRepository = new BaseRepository<ProductCostHistory>(_context)));
        public IRepository<ProductDescription> ProductDescriptionRepository => _productDescriptionRepository ?? ((_productDescriptionRepository = new BaseRepository<ProductDescription>(_context)));
        public IRepository<ProductInventory> ProductInventoryRepository => _productInventoryRepository ?? ((_productInventoryRepository = new BaseRepository<ProductInventory>(_context)));
        public IRepository<ProductListPriceHistory> ProductListPriceHistoryRepository => _productListPriceHistoryRepository ?? ((_productListPriceHistoryRepository = new BaseRepository<ProductListPriceHistory>(_context)));
        public IRepository<ProductModel> ProductModelRepository => _productModelRepository ?? (_productModelRepository = new BaseRepository<ProductModel>(_context));
        public IRepository<ProductModelIllustration> ProductModelIllustrationRepository => _productModelIllustrationRepository ?? ((_productModelIllustrationRepository = new BaseRepository<ProductModelIllustration>(_context)));
        public IRepository<ProductModelProductDescriptionCulture> ProductModelProductDescriptionCultureRepository => _productModelProductDescriptionCultureRepository ?? (_productModelProductDescriptionCultureRepository = new BaseRepository<ProductModelProductDescriptionCulture>(_context));
        public IRepository<ProductPhoto> ProductPhotoRepository => _productPhotoRepository ?? (_productPhotoRepository = new BaseRepository<ProductPhoto>(_context));
        public IRepository<ProductReview> ProductReviewRepository => _productReviewRepository ?? (_productReviewRepository = new BaseRepository<ProductReview>(_context));
        public IRepository<ProductSubcategory> ProductSubcategoryRepository => _productSubcategoryRepository ?? (_productSubcategoryRepository = new BaseRepository<ProductSubcategory>(_context));
        public IRepository<ScrapReason> ScrapReasonRepository => _scrapReasonRepository ?? (_scrapReasonRepository = new BaseRepository<ScrapReason>(_context));
        public IRepository<TransactionHistory> TransactionHistoryRepository => _transactionHistoryRepository ?? (_transactionHistoryRepository = new BaseRepository<TransactionHistory>(_context));
        public IRepository<TransactionHistoryArchive> TransactionHistoryArchiveRepository => _transactionHistoryArchiveRepository ?? ((_transactionHistoryArchiveRepository = new BaseRepository<TransactionHistoryArchive>(_context)));
        public IRepository<UnitMeasure> UnitMeasureRepository => _unitMeasureRepository ?? (_unitMeasureRepository = new BaseRepository<UnitMeasure>(_context));
        public IRepository<WorkOrder> WorkOrderRepository => _workOrderRepository ?? (_workOrderRepository = new BaseRepository<WorkOrder>(_context));
        public IRepository<WorkOrderRouting> WorkOrderRoutingRepository => _workOrderRoutingRepository ?? (_workOrderRoutingRepository = new BaseRepository<WorkOrderRouting>(_context));

        public void Commit()
        {
            _context.SaveChanges();
        }
    }
}
