// Copyright (c) Oleg Zudov. All Rights Reserved. Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.
// This file is based on or incorporates material from the project Selenium, licensed under the Apache License, Version 2.0. More info in THIRD-PARTY-NOTICES file.
using System.Threading;
using System.Threading.Tasks;
using Zu.WebBrowser.AsyncInteractions;

namespace Zu.AsyncWebDriver.Interactions
{
    public class BuiltAction : IAction
    {
        private IWebDriver driver;
        private ActionBuilder actionBuilder;
        private CompositeAction action;
        public BuiltAction(IWebDriver driver, ActionBuilder actionBuilder, CompositeAction action)
        {
            this.driver = driver;
            this.actionBuilder = actionBuilder;
            this.action = action;
        }

        public async Task Perform(CancellationToken cancellationToken = default (CancellationToken))
        {
            IActionExecutor actionExecutor = this.driver as IActionExecutor;
            if (await actionExecutor.IsActionExecutor(cancellationToken).ConfigureAwait(false))
            {
                await actionExecutor.PerformActions(this.actionBuilder.ToActionSequenceList()).ConfigureAwait(false);
            }
            else
            {
                await this.action.Perform().ConfigureAwait(false);
            }
        }
    }
}