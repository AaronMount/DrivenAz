using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DrivenAz.Public;

namespace DrivenAz.Internal
{
   internal class EventHub
   {
      public void RaiseOperationCompleted(DrivenAzOperationCompletedArgs args)
      {
         OperationCompleted(this, args);
      }
      
      public event EventHandler<DrivenAzOperationCompletedArgs> OperationCompleted = delegate { };
   }
}
