namespace RPG.Helper
{
    /// <summary>
    /// The DeferredValue class is a container that wraps a
    /// value of type T and ensures that the value is initialized
    /// just before it is first accessed. The value is initialized
    /// by calling a delegate function provided at the time the
    /// object is constructed.
    ///  
    /// The purpose of this class is to provide a way to delay the
    /// initialization of a value until it is actually needed.
    /// This can be useful in situations where the initialization
    /// of the value is costly or time-consuming, and you want to
    /// avoid initializing it unless it is actually needed. For example,
    /// you might use this class to initialize a complex object or to
    /// load data from a file or database when it is first accessed,
    /// rather than at the time the object is constructed.
    ///  
    /// The DeferredValue class has a single public property called
    /// Value, which allows you to get or set the wrapped value.
    /// When you access the Value property for the first time, the
    /// DeferredValue class checks whether the value has already been
    /// initialized. If it has not been initialized, the class calls the
    /// initializer delegate to initialize the value, and then stores
    /// the initialized value in an internal field. Subsequent
    /// accesses to the Value property will return the stored
    /// value without re-initializing it.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class CDeferredValue<T>
    {
        private bool initialized = false;
        private readonly InitializerDelegate initializer;

        /// <summary>
        /// Get or set the contents of this container.
        /// </summary>
        /// <remarks>
        /// Note that setting the value before initialization will initialize 
        /// the class.
        /// </remarks>
        private T value;
        public T Value
        {
            get
            {
                EnsureInitialized();
                return this.value;
            }
            set
            {
                initialized = true;
                this.value = value;
            }
        }

        /// <summary>
        /// Delegate type for the initializer function.
        /// </summary>
        public delegate T InitializerDelegate();

        /// <summary>
        /// Construct a new instance of the `LazyValue` class.
        /// </summary>
        /// <param name="initializer">
        /// The initializer delegate to call when the value is first accessed.
        /// </param>
        public CDeferredValue(InitializerDelegate initializer)
        {
            this.initializer = initializer;
        }

        /// <summary>
        /// Ensure that the value has been initialized.
        /// </summary>
        /// <remarks>
        /// If the value has already been initialized, this method does nothing.
        /// Otherwise, it initializes the value by calling the initializer delegate.
        /// </remarks>
        public void EnsureInitialized()
        {
            if (!initialized)
            {
                this.value = initializer();
                initialized = true;
            }
        }
    }
}
