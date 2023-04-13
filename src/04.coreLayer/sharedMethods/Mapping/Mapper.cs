using System.Collections;
using System.Reflection;

namespace SharedMethods.Mapping
{
    public static class Mapper
    {

        public static IList<TDestination>? MapList<TSource, TDestination>(IList<TSource>? source)
        {
            if (source is null)
                return null;

            var destination = Activator.CreateInstance<List<TDestination>>();

            foreach (var sourceItem in source)
            {
                var destinationItem = Activator.CreateInstance<TDestination>();

                foreach (PropertyInfo sourceProp in typeof(TSource).GetProperties())
                {
                    PropertyInfo destinationProp = typeof(TDestination).GetProperty(sourceProp.Name)!;

                    if (destinationProp != null && destinationProp.CanWrite)
                    {
                        var sourceValue = sourceProp.GetValue(sourceItem);
                        var destinationValue = sourceValue;

                        if (sourceValue != null)
                        {
                            if (!destinationProp.PropertyType.IsAssignableFrom(sourceProp.PropertyType))
                            {
                                if (destinationProp.PropertyType.IsEnum && sourceProp.PropertyType == typeof(string))
                                {
                                    destinationValue = Enum.Parse(destinationProp.PropertyType, (string)sourceValue);
                                }
                                else if (destinationProp.PropertyType.IsGenericType && destinationProp.PropertyType.GetGenericTypeDefinition() == typeof(List<>))
                                {
                                    destinationValue = CreateMapList(sourceValue, destinationProp.PropertyType.GetGenericArguments()[0]);
                                }
                                else if (destinationProp.PropertyType.IsClass)
                                {
                                    destinationValue = Map(sourceValue, destinationProp.PropertyType);
                                }
                            }
                        }

                        destinationProp.SetValue(destinationItem, destinationValue);
                    }
                }

                destination.Add(destinationItem);
            }

            return destination;
        }
        public static TDestination? Map<TSource, TDestination>(TSource source)
        {
            if (source == null)
            {
                return default;
            }

            var destinationItem = Activator.CreateInstance<TDestination>();

            foreach (PropertyInfo sourceProp in typeof(TSource).GetProperties())
            {
                PropertyInfo destinationProp = typeof(TDestination).GetProperty(sourceProp.Name)!;

                if (destinationProp != null && destinationProp.CanWrite)
                {
                    var sourceValue = sourceProp.GetValue(source);
                    var destinationValue = sourceValue;

                    if (sourceValue != null)
                    {
                        if (!destinationProp.PropertyType.IsAssignableFrom(sourceProp.PropertyType))
                        {
                            if (destinationProp.PropertyType.IsEnum && sourceProp.PropertyType == typeof(string))
                            {
                                destinationValue = Enum.Parse(destinationProp.PropertyType, (string)sourceValue);
                            }
                            else if (destinationProp.PropertyType.IsGenericType && destinationProp.PropertyType.GetGenericTypeDefinition() == typeof(List<>))
                            {
                                destinationValue = CreateMapList(sourceValue, destinationProp.PropertyType.GetGenericArguments()[0]);
                            }
                            else if (destinationProp.PropertyType.IsClass)
                            {
                                destinationValue = Map(sourceValue, destinationProp.PropertyType);
                            }
                        }
                    }

                    destinationProp.SetValue(destinationItem, destinationValue);
                }
            }

            return destinationItem;
        }

        private static object? CreateMapList(object sourceList, Type destinationType)
        {
            IList destinationList = (IList)Activator.CreateInstance(typeof(List<>).MakeGenericType(destinationType))!;

            foreach (object sourceItem in (IEnumerable)sourceList)
            {
                object? destinationItem = Map(sourceItem, destinationType);
                if (destinationItem is not null && destinationList is not null)
                {
                    destinationList.Add(destinationItem);
                }
            }

            return destinationList;
        }

        private static object? Map(object source, Type destinationType)
        {
            var destination = Activator.CreateInstance(destinationType);
            var sourceProps = source.GetType().GetProperties();
            var destinationProps = destinationType.GetProperties();

            foreach (var sourceProp in sourceProps)
            {
                var destinationProp = destinationProps.FirstOrDefault(x => x.Name == sourceProp.Name);

                if (destinationProp != null && destinationProp.CanWrite)
                {
                    object? sourceValue = sourceProp.GetValue(source, null);
                    object? destinationValue = null;

                    if (sourceValue != null)
                    {
                        if (destinationProp.PropertyType.IsAssignableFrom(sourceProp.PropertyType))
                        {
                            destinationValue = sourceValue;
                        }
                        else if (destinationProp.PropertyType.IsEnum && sourceProp.PropertyType == typeof(string))
                        {
                            destinationValue = Enum.Parse(destinationProp.PropertyType, (string)sourceValue);
                        }
                        else if (destinationProp.PropertyType.IsGenericType && destinationProp.PropertyType.GetGenericTypeDefinition() == typeof(List<>))
                        {
                            destinationValue = CreateMapList(sourceValue, destinationProp.PropertyType.GetGenericArguments()[0]);
                        }
                        else if (destinationProp.PropertyType.IsClass)
                        {
                            destinationValue = Map(sourceValue, destinationProp.PropertyType);
                        }
                    }

                    destinationProp.SetValue(destination, destinationValue, null);
                }
            }

            return destination;
        }
    }
}