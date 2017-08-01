// Copyright (c) Oleg Zudov. All Rights Reserved. Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.
// This file is based on or incorporates material from the project Selenium, licensed under the Apache License, Version 2.0. More info in THIRD-PARTY-NOTICES file.

using System;
using Zu.AsyncWebDriver.Internal;
using Zu.WebBrowser.AsyncInteractions;

namespace Zu.AsyncWebDriver.Interactions
{
    /// <summary>
    ///     Provides a mechanism for building advanced interactions with the browser.
    /// </summary>
    public class TouchActions : Actions
    {
        private readonly ITouchScreen touchScreen;

        /// <summary>
        ///     Initializes a new instance of the <see cref="TouchActions" /> class.
        /// </summary>
        /// <param name="driver">The <see cref="IWebDriver" /> object on which the actions built will be performed.</param>
        public TouchActions(IWebDriver driver) : base(driver)
        {
            var touchScreenDriver = driver as IHasTouchScreen;
            if (touchScreenDriver == null)
            {
                var wrapper = driver as IWrapsDriver;
                while (wrapper != null)
                {
                    touchScreenDriver = wrapper.WrappedDriver as IHasTouchScreen;
                    if (touchScreenDriver != null)
                        break;
                    wrapper = wrapper.WrappedDriver as IWrapsDriver;
                }
            }

            if (touchScreenDriver == null)
                throw new ArgumentException(
                    "The IWebDriver object must implement or wrap a driver that implements IHasTouchScreen.", "driver");
            touchScreen = touchScreenDriver.TouchScreen;
        }

        /// <summary>
        ///     Taps the touch screen on the specified element.
        /// </summary>
        /// <param name="onElement">The element on which to tap.</param>
        /// <returns>A self-reference to this <see cref="TouchActions" />.</returns>
        public TouchActions SingleTap(IWebElement onElement)
        {
            var locatable = GetLocatableFromElement(onElement);
            AddAction(new SingleTapAction(touchScreen, locatable));
            return this;
        }

        /// <summary>
        ///     Presses down at the specified location on the screen.
        /// </summary>
        /// <param name="locationX">The x coordinate relative to the view port.</param>
        /// <param name="locationY">The y coordinate relative to the view port.</param>
        /// <returns>A self-reference to this <see cref="TouchActions" />.</returns>
        public TouchActions Down(int locationX, int locationY)
        {
            AddAction(new ScreenPressAction(touchScreen, locationX, locationY));
            return this;
        }

        /// <summary>
        ///     Releases a press at the specified location on the screen.
        /// </summary>
        /// <param name="locationX">The x coordinate relative to the view port.</param>
        /// <param name="locationY">The y coordinate relative to the view port.</param>
        /// <returns>A self-reference to this <see cref="TouchActions" />.</returns>
        public TouchActions Up(int locationX, int locationY)
        {
            AddAction(new ScreenReleaseAction(touchScreen, locationX, locationY));
            return this;
        }

        /// <summary>
        ///     Moves to the specified location on the screen.
        /// </summary>
        /// <param name="locationX">The x coordinate relative to the view port.</param>
        /// <param name="locationY">The y coordinate relative to the view port.</param>
        /// <returns>A self-reference to this <see cref="TouchActions" />.</returns>
        public TouchActions Move(int locationX, int locationY)
        {
            AddAction(new ScreenMoveAction(touchScreen, locationX, locationY));
            return this;
        }

        /// <summary>
        ///     Scrolls the touch screen beginning at the specified element.
        /// </summary>
        /// <param name="onElement">The element on which to begin scrolling.</param>
        /// <param name="offsetX">The x coordinate relative to the view port.</param>
        /// <param name="offsetY">The y coordinate relative to the view port.</param>
        /// <returns>A self-reference to this <see cref="TouchActions" />.</returns>
        public TouchActions Scroll(IWebElement onElement, int offsetX, int offsetY)
        {
            var locatable = GetLocatableFromElement(onElement);
            AddAction(new ScrollAction(touchScreen, locatable, offsetX, offsetY));
            return this;
        }

        /// <summary>
        ///     Double-taps the touch screen on the specified element.
        /// </summary>
        /// <param name="onElement">The element on which to double-tap.</param>
        /// <returns>A self-reference to this <see cref="TouchActions" />.</returns>
        public TouchActions DoubleTap(IWebElement onElement)
        {
            var locatable = GetLocatableFromElement(onElement);
            AddAction(new DoubleTapAction(touchScreen, locatable));
            return this;
        }

        /// <summary>
        ///     Presses and holds on the touch screen on the specified element.
        /// </summary>
        /// <param name="onElement">The element on which to press and hold</param>
        /// <returns>A self-reference to this <see cref="TouchActions" />.</returns>
        public TouchActions LongPress(IWebElement onElement)
        {
            var locatable = GetLocatableFromElement(onElement);
            AddAction(new LongPressAction(touchScreen, locatable));
            return this;
        }

        /// <summary>
        ///     Scrolls the touch screen to the specified offset.
        /// </summary>
        /// <param name="offsetX">The horizontal offset relative to the view port.</param>
        /// <param name="offsetY">The vertical offset relative to the view port.</param>
        /// <returns>A self-reference to this <see cref="TouchActions" />.</returns>
        public TouchActions Scroll(int offsetX, int offsetY)
        {
            AddAction(new ScrollAction(touchScreen, offsetX, offsetY));
            return this;
        }

        /// <summary>
        ///     Flicks the current view.
        /// </summary>
        /// <param name="speedX">The horizontal speed in pixels per second.</param>
        /// <param name="speedY">The vertical speed in pixels per second.</param>
        /// <returns>A self-reference to this <see cref="TouchActions" />.</returns>
        public TouchActions Flick(int speedX, int speedY)
        {
            AddAction(new FlickAction(touchScreen, speedX, speedY));
            return this;
        }

        /// <summary>
        ///     Flicks the current view starting at a specific location.
        /// </summary>
        /// <param name="onElement">The element at which to start the flick.</param>
        /// <param name="offsetX">The x offset relative to the viewport.</param>
        /// <param name="offsetY">The y offset relative to the viewport.</param>
        /// <param name="speed">The speed in pixels per second.</param>
        /// <returns>A self-reference to this <see cref="TouchActions" />.</returns>
        public TouchActions Flick(IWebElement onElement, int offsetX, int offsetY, int speed)
        {
            var locatable = GetLocatableFromElement(onElement);
            AddAction(new FlickAction(touchScreen, locatable, offsetX, offsetY, speed));
            return this;
        }
    }
}