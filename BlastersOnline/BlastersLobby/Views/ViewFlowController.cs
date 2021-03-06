﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Awesomium.Core;
using Awesomium.Windows.Forms;

namespace BlastersLobby.Views
{
    /// <summary>
    /// The ViewFlowController controls the web view controller and directs things as neccessary 
    /// </summary>
    public class ViewFlowController
    {

        /// <summary>
        /// The WebControl context that is available to this <see cref="ViewFlowController"/>.
        /// </summary>
        public WebControl WebControl { get; set; }

        public View CurrentView { get; set; }

        public ViewFlowController(WebControl webControl)
        {
            WebControl = webControl;
            WebControl.ConsoleMessage += WebControlOnConsoleMessage;
        }

        private void WebControlOnConsoleMessage(object sender, ConsoleMessageEventArgs consoleMessageEventArgs)
        {
            Console.WriteLine(consoleMessageEventArgs.Message);
        }

        /// <summary>
        /// Changes the view suddently to the new specificed one without any context change warning.
        /// </summary>
        /// <param name="view">The view to switch the context to</param>
        public void ChangeView(View view)
        {

            if (CurrentView != null)
                CurrentView.OnClose();

            CurrentView = view;


            // Assign the flow controller
            view.FlowController = this;

            // Load up the document page
            view.OnViewAppeared();



        }

    }
}
