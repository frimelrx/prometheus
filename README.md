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

Commands Ran For Setup:
- dotnet new webapi --framework net8.0
- dotnet add package DotNetEnv

### INSIDE THE .ENV File
I put the API key in there with "API_KEY" being the variable name. It's one thing to expose a API key to the public 🙂.

# To Run the Program:
- dotnet run 
### It will listen on port 5000, this can be modified in the Program.cs

Example Of API Calls:

[localhost:5000/api/stocks/IBM](localhost:5000/api/stocks/IBM)

[localhost:5000/api/stocks/AMD](localhost:5000/api/stocks/AMD)

[localhost:5000/api/stocks/POS](localhost:5000/api/stocks/POS) <- Not a real stock
