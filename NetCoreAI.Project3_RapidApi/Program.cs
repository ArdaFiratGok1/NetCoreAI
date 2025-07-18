﻿using System.Net.Http.Headers;
using System.Text.Json.Serialization;
using NetCoreAI.Project3_RapidApi.View_Models;
using Newtonsoft.Json;
var client = new HttpClient();
List<ApiSeriesViewModel> apiSeriesViewModels = new List<ApiSeriesViewModel>();
var request = new HttpRequestMessage
{
    Method = HttpMethod.Get,
    RequestUri = new Uri("https://imdb-top-100-movies.p.rapidapi.com/series/"),
    Headers =
    {
        { "x-rapidapi-key", "cb1b21613bmshe30813926cf23eap100ce7jsnffc9be194a08" },
        { "x-rapidapi-host", "imdb-top-100-movies.p.rapidapi.com" },
    },
};
using (var response = await client.SendAsync(request))
{
    response.EnsureSuccessStatusCode();
    var body = await response.Content.ReadAsStringAsync();
    apiSeriesViewModels=JsonConvert.DeserializeObject<List<ApiSeriesViewModel>>(body);

    foreach(var series in apiSeriesViewModels)
    {
        Console.WriteLine($"Rank: {series.rank}  -  Title: {series.title} - Rating: {series.rating} - Year: {series.year}");

    }
}