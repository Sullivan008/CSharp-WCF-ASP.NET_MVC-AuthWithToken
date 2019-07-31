using System;
using System.ServiceModel.Configuration;

namespace AuthWithTokenServer.Classes.IDispatchMessageInspectorFunctions
{
    /// <summary>
    ///     A web.config-ban kiterjesztett osztály, amely implementálja a
    ///     BehaviorExtensionElement absztrakt osztályt
    /// </summary>
    public class TokenValidationBehaviorExtension : BehaviorExtensionElement
    {
        #region BehaviorExtensionElement

        /// <summary>
        ///     Viselkedési típus definiálása.
        ///     Visszatérít egy TokenValidationServiceBehavior típusú osztályt
        /// </summary>
        public override Type BehaviorType
        {
            get { return typeof(TokenValidationServiceBehavior); }
        }

        /// <summary>
        ///     A jelenlegi konfugirációs beállítások alapján létrehoz egy
        ///     viselkedési bővítményt
        /// </summary>
        /// <returns>Az elkészített viselkedés-kiterjesztés</returns>
        protected override object CreateBehavior()
        {
            return new TokenValidationServiceBehavior();
        }

        #endregion
    }
}