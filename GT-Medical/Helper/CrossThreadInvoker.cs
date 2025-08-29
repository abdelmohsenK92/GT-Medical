using GT_Medical.Abstractions;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GT_Medical.Helper
{
    public class CrossThreadInvoker : ISingletonService
    {
        // UI marshaling (set once from the Form)
        private ISynchronizeInvoke _uiInvoker;
        private SynchronizationContext _uiCtx;

        public CrossThreadInvoker(ISynchronizeInvoke uiInvoker)
        {
            AttachUi(uiInvoker);
        }

        /// <summary>
        /// Capture UI context (call once from the UI thread, e.g., db.AttachUi(this) in the Form).
        /// </summary>
        public void AttachUi(ISynchronizeInvoke invoker)
          {
            if (_uiInvoker != null)
                return;
            _uiInvoker = invoker;
            _uiCtx = SynchronizationContext.Current; // when called from UI thread, this captures the UI context
        }
        /// <summary>
        /// Execute an action on the UI thread if needed.
        /// </summary>
        public void RunOnUi(Action action)
        {
            if (action == null) return;

            // Prefer WinForms invoker (Control/Form implements ISynchronizeInvoke)
            if (_uiInvoker != null && _uiInvoker.InvokeRequired)
            {
                _uiInvoker.Invoke(action, null);
                return;
            }

            // Or use captured UI SynchronizationContext (if AttachUi was called on UI thread)
            if (_uiCtx != null && SynchronizationContext.Current != _uiCtx)
            {
                _uiCtx.Send(_ => action(), null); // use Post for async if preferred
                return;
            }

            // Already on UI (or no context available)
            action();
        }
    }
}
