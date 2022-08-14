# Rollout

Rollout is a library design to be a simple to use **feature management** interface.
It uses redis as a storage by default, but it also gives you the freedom to implement your own storage.

## Features

- Check if a feature is active by percentage
- Check if a feature is active by user
- Check if a feature is active by group

### Add Rollout to your project

```csharp
var builder = WebApplication.CreateBuilder(args);

// Redis is necessary if you are using the default rollout storage
builder.Services.AddSingleton<IConnectionMultiplexer>(
    _ => ConnectionMultiplexer.Connect(builder.Configuration.GetConnectionString("Redis"))
);
services.AddRollout();
```

Add rollout with custom storage implementation:

```csharp
var builder = WebApplication.CreateBuilder(args);

// MyCustomStorageImplementation must implement the IFeatureStorage interface
services.AddRolloutWithCustomStorage<MyCustomStorageImplementation>();
```

### Check if a feature is active for everyone
A feature is active for everyone if the percentage is set to 100% or if the group `all` is set


```csharp
public class MyService
{
  private readonly IFeatureManager _featureManager;

  public MyService(IFeatureManager featureManager)
  {
    _featureManager = featureManager;
  }


  public async Task MyMethod() {
    bool result = await _featureManager.IsActiveFor("my_feature");
  }
}
```

### Check if a feature is active for user

A feature is active for a user if the user falls within the active percentage set the user is specificaly set with the `SetUsers` method

```csharp
public class MyService
{
  private readonly IFeatureManager _featureManager;

  public MyService(IFeatureManager featureManager)
  {
    _featureManager = featureManager;
  }


  public async Task MyMethod() {
    bool result = await _featureManager.IsActiveFor("my_feature", user: "my_user");
  }
}
```

### Check if a feature is active for group

```csharp
public class MyService
{
  private readonly IFeatureManager _featureManager;

  public MyService(IFeatureManager featureManager)
  {
    _featureManager = featureManager;
  }


  public async Task MyMethod() {
    bool result = await _featureManager.IsActiveFor("my_feature", group: "my_group");
  }
}
```

### Activate a feature for x% of users

```csharp
public class MyService
{
  private readonly IFeatureManager _featureManager;

  public MyService(IFeatureManager featureManager)
  {
    _featureManager = featureManager;
  }


  public async Task MyMethod() {
    await _featureManager.SetPercentage("my_feature", 47);
  }
}
```

### Activate a feature for specific users:

```csharp
public class MyService
{
  private readonly IFeatureManager _featureManager;

  public MyService(IFeatureManager featureManager)
  {
    _featureManager = featureManager;
  }


  public async Task MyMethod() {
    await _featureManager.SetUsers("my_feature", new[]{"User1", "User2"});
  }
}
```
The `SetUsers` method concatenates the new users with the preeexisting users

### Activate a feature for specific groups

```csharp
public class MyService
{
  private readonly IFeatureManager _featureManager;

  public MyService(IFeatureManager featureManager)
  {
    _featureManager = featureManager;
  }


  public async Task MyMethod() {
    await _featureManager.SetGroups("my_feature", new[]{"Group1", "Group2"});
  }
}
```
The `SetSetGroups` method concatenates the new groups with the preeexisting groups
