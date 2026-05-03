public class ServiceDescriptor
{
    public Type ServiceType { get; set; }
    public Type ImplementationType { get; set; }
    public ServiceLifeTime LifeTime { get; set; }
    public ServiceDescriptor(Type serviceType, Type implementationType, ServiceLifeTime lifeTime)
    {
        ServiceType = serviceType;
        ImplementationType = implementationType;
        LifeTime = lifeTime;
    }
}
public enum ServiceLifeTime
{
    Singleton, 
    Scoped, 
    Transient
}