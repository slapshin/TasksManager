using System;
using System.Runtime.Serialization;
using System.Security.Permissions;
using System.Web.Http.ModelBinding;

namespace Web.SPA.Common
{
    [Serializable]
    public class ModelStateException : Exception
    {
        public ModelStateException(ModelStateDictionary modelState)
        {
            this.ModelState = modelState;
        }

        [SecurityPermissionAttribute(SecurityAction.Demand, SerializationFormatter = true)]
        protected ModelStateException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
            this.ModelState = (ModelStateDictionary)info.GetValue("ModelState", typeof(ModelStateDictionary));
        }

        public ModelStateDictionary ModelState { get; private set; }

        [SecurityPermissionAttribute(SecurityAction.Demand, SerializationFormatter = true)]
        public override void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            if (info == null)
            {
                throw new ArgumentNullException("info");
            }
            info.AddValue("ModelState", this.ModelState, typeof(ModelStateDictionary));
            base.GetObjectData(info, context);
        }
    }
}