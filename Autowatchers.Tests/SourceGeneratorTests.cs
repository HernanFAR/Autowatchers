using Autowatchers.Net7;
using Microsoft.Extensions.Logging;
using System.Reflection;
using Xunit.Sdk;

namespace Autowatchers.Tests;

public class SourceGeneratorTests
{
    [Fact]
    public void SourceGenerator_ShouldCreateAllMethodsThatAreNotDecoratedWithAutowatchIgnoreAttribute_DetailNormal()
    {
        // Arrange
        var validObservedClassProperties = typeof(DummyClass)
            .GetProperties(BindingFlags.Instance | BindingFlags.Public)
            .Where(e => e.CanRead)
            .Where(e => e.CanWrite)
            .Where(e =>
            {
                var mget = e.GetGetMethod(false);
                var mset = e.GetSetMethod(false);

                return mget is not null &&
                       mset is not null &&
                       !mset.IsInitOnly();
            })
            .Where(e =>
            {
                var attributes = e.CustomAttributes;

                return !attributes.Any() || attributes.Any(e => e.AttributeType != typeof(AutoWatchIgnoreAttribute));
            })
            .ToArray();

        var observerMembers = typeof(Net7.NormalWatchers.WithScopedNamespaceClassWatch)
            .GetMembers();
        var constructor = (ConstructorInfo)observerMembers.Single(e => e.MemberType == MemberTypes.Constructor);

        var constructorParameters = constructor.GetParameters();
        var constructorParameter = Assert.Single(constructorParameters);

        var observedObject = (PropertyInfo)observerMembers
            .Where(e => e.MemberType == MemberTypes.Property)
            .Single(e => e.Name == "Observed");

        // Assert
        Assert.Equal("observed", constructorParameter.Name);
        Assert.Equal(typeof(DummyClass), constructorParameter.ParameterType);
        Assert.Equal(typeof(DummyClass), observedObject.PropertyType);

        foreach (var observedPropertyInfo in validObservedClassProperties)
        {
            var propertyName = observedPropertyInfo.Name;
            var propertyType = observedPropertyInfo.PropertyType;

            var @event = (EventInfo)observerMembers
                .Where(e => e.MemberType == MemberTypes.Event)
                .Single(e => e.Name == $"{propertyName}Changed");

            var property = (PropertyInfo)observerMembers
                .Where(e => e.MemberType == MemberTypes.Property)
                .Single(e => e.Name == propertyName);

            var method = (MethodInfo)observerMembers
                .Where(e => e.MemberType == MemberTypes.Method)
                .Single(e => e.Name == $"AddImmediate{propertyName}ChangedEvent");

            var methodParameters = method.GetParameters();
            var methodParameter = Assert.Single(methodParameters);
            var actionType = typeof(Action<,>).MakeGenericType(propertyType, propertyType);

            Assert.Equal(typeof(void), method.ReturnType);
            Assert.Equal(actionType, methodParameter.ParameterType);
            Assert.Equal(actionType, @event.EventHandlerType);
            Assert.Equal(observedPropertyInfo.PropertyType, property.PropertyType);

        }
    }

    [Fact]
    public void SourceGenerator_ShouldCreateAllMethodsThatAreNotDecoratedWithAutowatchIgnoreAttribute_DetailDeep()
    {
        // Arrange
        var validObservedClassProperties = typeof(DummyClass)
            .GetProperties(BindingFlags.Instance | BindingFlags.Public)
            .Where(e => e.CanRead)
            .Where(e => e.CanWrite)
            .Where(e =>
            {
                var mget = e.GetGetMethod(false);
                var mset = e.GetSetMethod(false);

                return mget is not null &&
                       mset is not null &&
                       !mset.IsInitOnly();
            })
            .Where(e =>
            {
                var attributes = e.CustomAttributes;

                return !attributes.Any() || attributes.Any(e => e.AttributeType != typeof(AutoWatchIgnoreAttribute));
            })
            .ToArray();

        var observerMembers = typeof(Net7.DeepWatchers.WithScopedNamespaceClassWatch)
            .GetMembers();

        var constructor = (ConstructorInfo)observerMembers.Single(e => e.MemberType == MemberTypes.Constructor);

        var constructorParameters = constructor.GetParameters();
        var constructorParameter = Assert.Single(constructorParameters);

        var observedObject = (PropertyInfo)observerMembers
            .Where(e => e.MemberType == MemberTypes.Property)
            .Single(e => e.Name == "Observed");

        var actionType = typeof(Action<,,>).MakeGenericType(typeof(DummyClass), typeof(DummyClass), typeof(string));

        var propertyChange = (EventInfo)observerMembers
            .Where(e => e.MemberType == MemberTypes.Event)
            .Single(e => e.Name == "PropertyChanged");

        var addImmediateEventMethod = (MethodInfo)observerMembers
            .Where(e => e.MemberType == MemberTypes.Method)
            .Single(e => e.Name == "AddImmediatePropertyChangedEvent");
        var parameters = addImmediateEventMethod.GetParameters();
        var parameter = Assert.Single(parameters);

        // Assert
        Assert.Equal("observed", constructorParameter.Name);
        Assert.Equal(typeof(DummyClass), constructorParameter.ParameterType);
        Assert.Equal(typeof(DummyClass), observedObject.PropertyType);
        Assert.Equal(actionType, propertyChange.EventHandlerType);
        Assert.Equal(actionType, parameter.ParameterType);

        foreach (var observedPropertyInfo in validObservedClassProperties)
        {
            var propertyName = observedPropertyInfo.Name;

            var property = (PropertyInfo)observerMembers
                .Where(e => e.MemberType == MemberTypes.Property)
                .Single(e => e.Name == propertyName);

            Assert.Equal(observedPropertyInfo.PropertyType, property.PropertyType);

        }
    }
}
