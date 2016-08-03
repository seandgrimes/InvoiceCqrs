$cmd = "packages\FluentMigrator.1.6.2\tools\Migrate.exe"
$dllPath = "InvoiceCqrs.Migrations\bin\Debug\InvoiceCqrs.Migrations.dll";
$param = '-conn', 'default', '--dbType', 'SqlServer2014', '-a', $dllPath

& $cmd $param