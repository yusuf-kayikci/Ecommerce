using Ecommerce.Common.Core;
using Ecommerce.Common.Models;

namespace Ecommerce.Common.Core
{
    public interface IUnitOfWork
    {

        IRepository<BillOfMaterials> BillOfMaterialsRepository { get; }
        IRepository<Culture> CultureRepository { get; }
        IRepository<Illustration> IllustrationRepository { get; }
        IRepository<Location> LocationRepository { get; }
        IRepository<Product> ProductRepository { get; }
        IRepository<ProductCategory> ProductCategoryRepository { get; }
        IRepository<ProductCostHistory> ProductCostHistoryRepository { get; }
        IRepository<ProductDescription> ProductDescriptionRepository { get; }
        IRepository<ProductInventory> ProductInventoryRepository { get; }
        IRepository<ProductListPriceHistory> ProductListPriceHistoryRepository { get; }
        IRepository<ProductModel> ProductModelRepository { get; }
        IRepository<ProductModelIllustration> ProductModelIllustrationRepository { get; }
        IRepository<ProductModelProductDescriptionCulture> ProductModelProductDescriptionCultureRepository { get; }
        IRepository<ProductPhoto> ProductPhotoRepository { get; }
        IRepository<ProductReview> ProductReviewRepository { get; }
        IRepository<ProductSubcategory> ProductSubcategoryRepository { get; }
        IRepository<ScrapReason> ScrapReasonRepository { get; }
        IRepository<TransactionHistory> TransactionHistoryRepository { get; }
        IRepository<TransactionHistoryArchive> TransactionHistoryArchiveRepository { get; }
        IRepository<UnitMeasure> UnitMeasureRepository { get; }
        IRepository<WorkOrder> WorkOrderRepository { get; }
        IRepository<WorkOrderRouting> WorkOrderRoutingRepository { get; }
        void Commit();
        void Rollback();
    }

}
