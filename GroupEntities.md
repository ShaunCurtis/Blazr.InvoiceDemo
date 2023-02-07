# Group Entities And Notification

Data and objects in the solution are divided into *GroupEntities*.  You will see this in the folder structure.  A *Group Entity* is a collection of objects that belong to a common entity.  For example the Core domain `WeatherForecast` group entity contains all the data classes and services associated with a Weather Forecast.

Each Group Entity has a `IEntityService` class that is used to identify it.

```csharp
public interface IEntityService { }
```

The Weather Forecast concrete implementation.

```csharp
public class WeatherForecastEntityService : IEntityService {}
```

Why do we need these?  They don't do anything.

Consider a data edit.  The user clicks on an edit putton on a row in the list form.  It pops up a edit dialog and the user changes the data and exits.  How does the list form know that a value in the list has changed?  It needs a notification.

The Notification Service definition:

```csharp
public interface INotificationService<TEntityService>
    where TEntityService : class, IEntityService
{
    public event EventHandler? ListChanged;
    public event EventHandler<RecordChangedEventArgs>? RecordChanged;

    public void NotifyListChanged(object? sender);
    public void NotifyRecordChanged(object? sender, object record);
```

And the implementation:

```csharp
public class NotificationService<TEntityService> : INotificationService<TEntityService>
    where TEntityService : class, IEntityService
{
    public event EventHandler? ListChanged;
    public event EventHandler<RecordChangedEventArgs>? RecordChanged;

    public void NotifyListChanged(object? sender)
        => this.ListChanged?.Invoke(sender, EventArgs.Empty);

    public void NotifyRecordChanged(object? sender, object record)
        => this.RecordChanged?.Invoke(sender, RecordChangedEventArgs.Create(record));
}
```

We can now define a Notification Service for each Group Entity like this:

```csharp
services.AddScoped<INotificationService<WeatherForecastEntityService>, NotificationService<WeatherForecastEntityService>>();
```

No concrete class defined.  The edit presenter injects the instance defined in DI and calls `NotifyRecordChanged` to raise the `RecordChanged` event.  The list presenter injects the same notification instance and registers a handler on the `RecordChanged` event.  It does whatever it needs to do and calls `NotifyListChanged`....



 