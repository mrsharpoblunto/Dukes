using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Mindscape.LightSpeed;

namespace DukesServer.MVP.Model
{
    internal class ModelContext
    {
        public static LightSpeedContext<ModelUnitOfWork> Current
        {
            set
            {
                ServiceLocator.Register<LightSpeedContext<ModelUnitOfWork>>(value);
            }
            get
            {
                return ServiceLocator.Get<LightSpeedContext<ModelUnitOfWork>>();
            }
        }
    }
}
