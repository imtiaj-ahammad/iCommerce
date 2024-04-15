namespace eCommerce.Domain;

public abstract class EntityBase
    {
        public Guid Id { get; set; }
        public virtual Guid CreatedBy { get; set; }
        public virtual DateTime CreateDate { get; set; }
        public virtual DateTime LastUpdateDate { get; set; }
        public virtual Guid LastUpdatedBy { get; set; }
        public bool IsMarkedToDelete { get; set; }
        public virtual DateTime DeletedDate { get; set; }
        public virtual Guid DeletedBy { get; set; }
        public virtual string Remarks {get; set;}
    }
