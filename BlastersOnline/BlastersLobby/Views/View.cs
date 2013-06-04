using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Awesomium.Core;

namespace BlastersLobby.Views
{
    /// <summary>
    /// The view is a state in the lobby application that is to be displayed.
    /// </summary>
    public abstract class View
    {


        /// <summary>
        /// This method is called when a view is about to appear
        /// </summary>
        public abstract void OnViewAppeared();

        /// <summary>
        /// This function should be called when a change to a model is made warrants updating the view
        /// </summary>
        public abstract void UpdateView();

        /// <summary>
        /// The master flow controller this view belongs to
        /// </summary>
        public ViewFlowController FlowController { get; set; }

        /// <summary>
        /// Executes a piece of JavaScript on the DOM
        /// </summary>
        /// <param name="functionName"></param>
        /// <param name="param"></param>
        public void ExecuteJavascriptViaString(string functionName, List<string> param)
        {
            JSObject window = FlowController.WebControl.ExecuteJavascriptWithResult("window");
            string paramSet = String.Join("|", param);

            var funcs = window.GetMethodNames();

            if (!window.HasMethod(functionName))
                throw new NotImplementedException("The given function is not implemented.");

            using (window)
            {
                window.Invoke(functionName, paramSet);
            }
        }

    }
}
