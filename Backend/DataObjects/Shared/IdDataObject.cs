using System;
namespace SocialNet.Backend.DataObjects
{
    public abstract partial class IdDataObject : DataObject
    {
        public string Id { get; set; }

        public string AppId { get; set; }

        public DateTime Created { get; set; }

        public DateTime Updated { get; set; }

        public bool IsNew
        {
            get { return string.IsNullOrWhiteSpace(Id); }
        }

        public override string ToString()
        {
            return String.Format("Id : {0}, Created : {1}, Last Updated : {2}", Id ?? "", Created, Updated);
        }

        public override void CopyTo(DataObject target)
        {
            var temp = target as IdDataObject;
            if(temp == null) throw new ArgumentException("target");

            base.CopyTo(temp);

            temp.Id = Id;
            temp.AppId = AppId;
            temp.Created = Created;
            temp.Updated = Updated;
        }
    }
}
