# Weather app

This application is a weather application which uses Current Weather Data from https://openweathermap.org/current. The frontend is written in Next.Js and backend is written in dotnet.

## Backend / API

Folder Weather.Api holds the APIs for this project. This project can be build and run using the following commands in terminal. or from relevant UI buttons.
`dotnet restore`
`dotnet build`
`dotnet run`

API uses rate limiter which should only allow 5 requests in 1 hour time. After the number of requests is exausted the API will return 429 status code to indicate too many requests.
Following 5 keys are available to use. API expect these api keys to be present in header names `x-api-key`. Without valid `x-api-key`, the API will return 401 to indicate unauthorized request.
`v39HoNutrLfRlj04UtHeJerut1LswoS4`
`wRokugif2sehoprumifrut2so7iglpos`
`yiTevAWriCLNaKlxAf9ostEviNL7rlb1`
`w4AcHibawublzOs0cIpLsTlJAdobreki`
`ph4wr97efrASTOHasetaqICasIpRuc96`

To test the API, need to go to `Weather.Api.Tests` directory and simply run `dotnet test`.

## Frontend application

The frontend application of this project is developed using `NextJs` which currently has only one homepage. There is an option to enter city name and choose country name from the list before clicking `Search` button, that will call the API (explained above) and display the appropriate result.

To the frontend application

1. cd into `weather-app`
2. run `npm install`
3. run `npm run dev`
   This will start the website in `http:localhost:3000`.

### To test the frontend application

1. cd into `weather-app`
2. run `npm install`
3. run `npm run test`
   This will run the test files that are setup inside `tests` directory.
