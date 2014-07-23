namespace Agile.WinUI
{
    /// <summary>
    /// GUI factory that is used when no other GUI factory can be found.
    /// </summary>
    public class ObjectGUIFactory : AgileGUIFactoryBase
    {
        #region Constructors and Factories

        /// <summary>
        /// Private Constructor. Use the Instantiate method to instantiate.
        /// </summary>
        private ObjectGUIFactory()
        {
        }

        /// <summary>
        /// Private Constructor. Use the Instantiate method to instantiate.
        /// </summary>
        private ObjectGUIFactory(object item) : base(item)
        {
        }

        /// <summary>
        /// Instantiates a new ObjectGUIFactory.
        /// </summary>
        /// <returns></returns>
        public static ObjectGUIFactory Build()
        {
            return new ObjectGUIFactory();
        }

        /// <summary>
        /// Instantiates a new ObjectGUIFactory.
        /// </summary>
        /// <returns></returns>
        public static ObjectGUIFactory Build(object item)
        {
            return new ObjectGUIFactory(item);
        }

        //   /// <summary>
        //   /// Instantiates a new ObjectGUIFactory.
        //   /// </summary>
        //   /// <returns></returns>
        //   public static ObjectGUIFactory Build(object item, object secondItem)
        //   {
        //       return new ObjectGUIFactory(item);
        //   }

        #endregion
    }
}