# Mohsin Khan-Senior Developer 

## Technical questions

1. How long did you spend on the coding test? What would you add to your solution if you had more time? If you didn't spend much time on the coding test then use this as an opportunity to explain what you would add.  
**Answer:** * *I spent 2h 30m on this test. If I had more time, I would have added some more test cases. Also, as I have lost some touch with the front end, to save time, I have displayed expected result in json format.* *  


2. What was the most useful feature that was added to the latest version of your chosen language? Please include a snippet of code that shows how you've used it.  
**Answer:** * * I have made use of IHttpClientFactory to efficiently make use of HttpClient. I have used it in the startup file of the web application by making use of services.AddHttpClient which is part of .netcore's DependencyInjection feature  * *  
**Code snippet:**   
```
public void ConfigureServices(IServiceCollection services)
{
    services.AddControllersWithViews();
    services.Configure<BaseUrlsOptions>(Configuration.GetSection("BaseUrls"));
    services.Configure<RelativeUrlsOptions>(Configuration.GetSection("RelativeUrls"));
    services.Configure<CredentialsOptions>(Configuration.GetSection("Credentials"));

    services.AddHttpClient<IRestaurantFinder, RestaurantFinder>(client =>
    {
        var baseUrls = new BaseUrlsOptions();
        var creds = new CredentialsOptions();

        Configuration.GetSection("BaseUrls").Bind(baseUrls);
        Configuration.GetSection("Credentials").Bind(creds);

        var byteArray = Encoding.ASCII.GetBytes($"{creds.Username}:{creds.Password}");

        client.BaseAddress = new Uri(baseUrls.PublicJustEatApis);
        client.DefaultRequestHeaders
            .Accept
            .Add(new MediaTypeWithQualityHeaderValue("application/json"));

        client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(
            "Basic",
            Convert.ToBase64String(byteArray));

        client.DefaultRequestHeaders.Add("Accept-Tenant", "uk");
        client.DefaultRequestHeaders.Add("Accept-Language", "en-GB");
        client.DefaultRequestHeaders.Add("Host", "public.je-apis.com");


    });

}
```  
  
  
3. How would you track down a performance issue in production? Have you ever had to do this?  
**Answer:** * *In the past, I have tracked down performance issues by* *  
- Setting a baseline load test  
- Check application metrics (this needs logs and metrics setup in place)  
- Look out for calls that are taking more time  
- This could be due to unnecessary retrys to a service that is consistently failing  
- Fix consistently failing service to resolve the performance problem. This could require restarting VMs or upgrading to better compute if that is the root cause for service failure    
  
  
4. How would you improve the Just Eat APIs that you just used?  
**Answer:** * *The api is returning a lot of extra data that is propably specific to various user interfaces. I think it would be a good idea to only return restaurant details as part of this api and keep the json payload simple and smaller in size. It has different view data details that may not be required by all callers of this api. * *  
  
  
5. Please describe yourself using JSON.  
**Answer**  
```
{
   "FullName":"Mohsin Khan",
   "Gender":"Male",
   "Age":"Prefer not to say but I feel like 18",
   "BirthCountry":"India",
   "Nationality":"British",
   "Address":{
      "DoorNumber":null,
      "FirstLineOfAddress":"",
      "City":"Carshalton",
      "PostCode":"SM5"
   },
   "Married":"Yes",
   "NumberOfWives":1,
   "Kids":[
      {
         "Gender":"Male",
         "Age":7
      }
   ],
   "Languages":[
      {
         "Language":"English"
      },
      {
         "Language":"Hindi"
      },
      {
         "Language":"Marathi"
      },
      {
         "Language":"Urdu"
      }
   ],
   "SportsPlayed":[
      {
         "Name":"Boxing",
         "Level":"Was an amature but not just for fun"
      },
      {
         "Name":"Ice Skating",
         "Level":"Completed silver level at Alexandra Palace"
      },
      {
         "Name":"Snooker",
         "Level":"Fun level"
      },
      {
         "Name":"Table Tennis",
         "Level":"Fun level"
      }
   ]
}
```
