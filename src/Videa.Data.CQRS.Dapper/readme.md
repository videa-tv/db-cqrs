# Videa Datastore 

## If necessary, update your depedency injection container.

```<language>
var container = new Container(config =>
{
	config.For<IDatastoreConnectionProxy>().Use<DapperDatastoreConnectionProxy>();
	config.For<IDatastoreConnectionProxyFactory>().Use<DapperDatastoreConnectionProxyFactory>();
}
```

## Notes:
- Currently, the Dapper implementations are named DapperDatastoreConnectionProxy and DapperDatastoreConnectionProxyFactory 