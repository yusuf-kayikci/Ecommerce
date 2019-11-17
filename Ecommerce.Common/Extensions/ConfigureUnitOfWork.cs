using Ecommerce.Common.Core;
using Ecommerce.Common.Models;
using Microsoft.Extensions.DependencyInjection;


namespace Ecommerce.Common.Extensions
{
    public static class  Configure
    {
        public static void ConfigureUnitOfWork(this IServiceCollection services)
        {

            services.AddScoped<IUnitOfWork, UnitOfWork>();
            services.AddScoped<IRepository<BillOfMaterials> , BaseRepository<BillOfMaterials>>();
            services.AddScoped<IRepository<Culture>, BaseRepository<Culture>>();
            services.AddScoped<IRepository<Illustration>, BaseRepository<Illustration>>();
            services.AddScoped<IRepository<Location>, BaseRepository<Location>>();
            services.AddScoped<IRepository<Product>, BaseRepository<Product>>();
            services.AddScoped<IRepository<ProductCategory>, BaseRepository<ProductCategory>>();
            services.AddScoped<IRepository<ProductCostHistory>, BaseRepository<ProductCostHistory>>();
            services.AddScoped<IRepository<ProductDescription>, BaseRepository<ProductDescription>>();
            services.AddScoped<IRepository<ProductInventory>, BaseRepository<ProductInventory>>();
            services.AddScoped<IRepository<ProductListPriceHistory>, BaseRepository<ProductListPriceHistory>>();
            services.AddScoped<IRepository<ProductModel>, BaseRepository<ProductModel>>();
            services.AddScoped<IRepository<ProductModelIllustration>, BaseRepository<ProductModelIllustration>>();
            services.AddScoped<IRepository<ProductModelProductDescriptionCulture>, BaseRepository<ProductModelProductDescriptionCulture>>();
            services.AddScoped<IRepository<ProductPhoto>, BaseRepository<ProductPhoto>>();
            services.AddScoped<IRepository<ProductReview>, BaseRepository<ProductReview>>();
            services.AddScoped<IRepository<ProductSubcategory>, BaseRepository<ProductSubcategory>>();
            services.AddScoped<IRepository<ScrapReason>, BaseRepository<ScrapReason>>();
            services.AddScoped<IRepository<TransactionHistory>, BaseRepository<TransactionHistory>>();
            services.AddScoped<IRepository<TransactionHistoryArchive>, BaseRepository<TransactionHistoryArchive>>();
            services.AddScoped<IRepository<UnitMeasure>, BaseRepository<UnitMeasure>>();
            services.AddScoped<IRepository<WorkOrder>, BaseRepository<WorkOrder>>();
            services.AddScoped<IRepository<WorkOrderRouting>, BaseRepository<WorkOrderRouting>>();
        }

    }
}
