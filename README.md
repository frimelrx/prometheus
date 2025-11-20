# prometheus
Technical Assessment For PG by Robert Frimel


## Instructions Given:
see: https://www.alphavantage.co/documentation/

### Requirements

- create a self-hosted C# .Net 8+  Java or React solution

- consume the api and expose a new api that:

- takes the symbol as a string parameter

- queries the intraday data for last month

- assume the data is updated every 15 minutes

- groups by the day

- returns a json response in this format:

    ```
    [

        {

            "day": "2009-01-30",

            "lowAverage": 40.2958,

            "highAverage": 49.7534,

            "volume": 49073348

        },
    ...

Commands Ran:
- dotnet new webapi --framework net8.0
- dotnet add package DotNetEnv