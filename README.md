# audit-log

* Modelo de Log para web-api .NET >= 4.5
* Log captura dados request e response da aplicação capturando inclusive o body.
* A saida do log pode ser vista na janela output do visual studio.
* Verificar a janela task do visual studio para identificar o ponto de persistencia do Log.
* Raiz do repositorio contem arquivo audit-log.postman_collection.json para ser importado no postman para auxiliar nos testes.
* A propriedade HttpError somente é preenchida quando temos statuscode maior ou igual 500.

### Output
```json
{
  "LogId": "80000021-0000-fc00-b63f-84710c7967bb",
  "Application": "NAME APPLICATION",
  "User": "",
  "Machine": "MACHINE-NAME",
  "RequestIpAddress": "::1",
  "RequestContentBody": {
    "Id": "0",
    "Description": "Novo produto",
    "Price": 15000
  },
  "RequestUri": "http://localhost:56390/api/values",
  "RequestMethod": "POST",
  "ResponseStatusCode": 201,
  "RequestHeaders": {
    "Cache-Control": "no-cache",
    "Connection": "keep-alive",
    "Accept": "*/*",
    "Accept-Encoding": "gzip deflate",
    "Host": "localhost:56390",
    "User-Agent": "PostmanRuntime/7.1.1"
  },
  "ResponseHeaders": {
    "Content-Type": "application/json; charset=utf-8"
  },
  "ResponseContentBody": {
    "id": 6,
    "description": "Novo produto",
    "price": 15000.0
  },
  "RequestTimestamp": "2018-01-07T15:03:17.2587699-02:00",
  "ResponseTimestamp": "2018-01-07T15:03:17.4944221-02:00",
  "TotalTime": "00:00:00.024",
  "HttpError": null
}
