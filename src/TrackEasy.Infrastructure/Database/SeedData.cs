using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using TrackEasy.Domain.Cities;
using TrackEasy.Domain.Connections;
using TrackEasy.Domain.DiscountCodes;
using TrackEasy.Domain.Discounts;
using TrackEasy.Domain.Operators;
using TrackEasy.Domain.Shared;
using TrackEasy.Domain.Stations;
using TrackEasy.Domain.Users;

namespace TrackEasy.Infrastructure.Database;

internal sealed class SeedData(IServiceProvider serviceProvider, TimeProvider timeProvider) : IHostedService
{
    public async Task StartAsync(CancellationToken cancellationToken)
    {
        using var scope = serviceProvider.CreateScope();
        
        var context = scope.ServiceProvider.GetRequiredService<TrackEasyDbContext>();
        var userManager = scope.ServiceProvider.GetRequiredService<UserManager<User>>();
        var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole<Guid>>>();
        
        await context.Database.MigrateAsync(cancellationToken);

        List<string> roles = [Roles.Admin, Roles.Manager, Roles.Passenger];
        foreach (var role in roles)
        {
            if (!await roleManager.RoleExistsAsync(role))
            {
                await roleManager.CreateAsync(new IdentityRole<Guid>(role));
            }
        }

        var adminUser = User.CreateAdmin("Admin", "Admin", "admin@admin.com", new DateOnly(1990, 1, 1), TimeProvider.System);
        
        if (await userManager.FindByEmailAsync(adminUser.Email!) is null)
        {
            await userManager.CreateAsync(adminUser, "Admin1234!");
            await userManager.AddToRoleAsync(adminUser, Roles.Admin);
        }
        
        var dbContext = scope.ServiceProvider.GetRequiredService<TrackEasyDbContext>();

        await dbContext.Database.ExecuteSqlAsync(
            $"""
             UPDATE AspNetUsers
             SET 
                 TwoFactorEnabled = 0,
                 EmailConfirmed = 1
             WHERE Email = 'admin@admin.com'
             """, cancellationToken);
        
        await SeedDiscounts(dbContext, cancellationToken);
        await SeedDiscountCodes(dbContext, cancellationToken);
        await SeedCities(dbContext, cancellationToken);
        await SeedStations(dbContext, cancellationToken);
        await SeedOperators(dbContext, cancellationToken);
        await SeedCoaches(dbContext, cancellationToken);
        await SeedTrains(dbContext, cancellationToken);
        await SeedConnections(dbContext, cancellationToken);
    }

    public Task StopAsync(CancellationToken cancellationToken) => Task.CompletedTask;

    private static async Task SeedDiscounts(TrackEasyDbContext context, CancellationToken cancellationToken)
    {
        List<Discount> discounts =
        [
            Discount.Create("Student", 51),
            Discount.Create("Senior", 30),
            Discount.Create("Veteran", 20),
            Discount.Create("Child", 50),
            Discount.Create("Family", 15)
        ];
        
        foreach (var discount in discounts)
        {
            if (!await context.Discounts.AnyAsync(d => d.Name == discount.Name, cancellationToken))
            {
                context.Discounts.Add(discount);
            }
        }
        
        await context.SaveChangesAsync(cancellationToken);
    }

    private  async Task SeedDiscountCodes(TrackEasyDbContext context, CancellationToken cancellationToken)
    {
        var dateTimeNow = TimeProvider.System.GetLocalNow().DateTime;
        
        List<DiscountCode> discountCodes =
        [
            DiscountCode.Create("SUMMER2025", 25, dateTimeNow.AddDays(1), dateTimeNow.AddMonths(5), timeProvider),
            DiscountCode.Create("WINTER2025", 30, dateTimeNow.AddDays(1), dateTimeNow.AddMonths(3), timeProvider),
            DiscountCode.Create("SPRING2025", 20, dateTimeNow.AddDays(1), dateTimeNow.AddMonths(4), timeProvider),
            DiscountCode.Create("FALL2025", 15, dateTimeNow.AddDays(1), dateTimeNow.AddMonths(6), timeProvider)
        ];
        
        foreach (var code in discountCodes)
        {
            if (!await context.DiscountCodes.AnyAsync(dc => dc.Code == code.Code, cancellationToken))
            {
                context.DiscountCodes.Add(code);
            }
        }
        
        await context.SaveChangesAsync(cancellationToken);
    }

    private static async Task SeedCities(TrackEasyDbContext context, CancellationToken cancellationToken)
    {
       List<City> cities =
        [
            City.Create("Starachowice", Country.PL, [
                "Powstały 1 IV 1939 r. z połączenia Wierzbnika i Starachowic-Fabrycznych.",
                "Słynęły z produkcji ciężarówek STAR w Zakładach FSC.",
                "Muzeum Przyrody i Techniki mieści się przy zabytkowym wielkim piecu."
            ]),
            City.Create("Wąchock", Country.PL, [
                "Średniowieczne Opactwo Cystersów (XII w.).",
                "Miasto żartów o sołtysie – stoi tu pomnik sołtysa.",
                "Prawa miejskie ponownie od 1994 r."
            ]),
            City.Create("Skarżysko-Kamienna", Country.PL, [
                "Rozwinęło się przy Kolei Iwangorodzko-Dąbrowskiej.",
                "Sanktuarium Matki Bożej Ostrobramskiej – kopia wileńskiej kaplicy.",
                "Muzeum Orła Białego prezentuje plenerową kolekcję militariów."
            ]),
            City.Create("Ostrowiec Świętokrzyski", Country.PL, [
                "Tradycje hutnicze sięgające XIX w. (Huta \"Ostrowiec\").",
                "Muzeum Historyczno-Archeologiczne w pałacu w Częstocicach.",
                "Leży nad rzeką Kamienną."
            ]),
            City.Create("Bodzentyn", Country.PL, [
                "Prawa miejskie z 1355 r. od Kazimierza Wielkiego.",
                "Ruiny gotyckiego zamku biskupów krakowskich.",
                "Punkt wyjścia szlaku na Łysicę (Świętokrzyski PN)."
            ]),
        
            City.Create("Kielce", Country.PL, [
                "Stolica woj. świętokrzyskiego od 1999 r.",
                "Barokowy Pałac Biskupów Krakowskich w centrum miasta.",
                "Rezerwat Kadzielnia z jaskiniami krasowymi w granicach miasta."
            ]),
            City.Create("Sandomierz", Country.PL, [
                "Średniowieczny układ urbanistyczny – \"Mały Rzym\".",
                "Podziemna trasa turystyczna w dawnych magazynach kupców.",
                "Serial „Ojciec Mateusz” kręcony tu od 2008 r."
            ]),
            City.Create("Busko-Zdrój", Country.PL, [
                "Uzdrowisko słynne z wód siarczkowych od XIX w.",
                "Park Zdrojowy zaprojektowany przez Henryka Marconiego.",
                "Międzynarodowy Festiwal Muzyczny im. Krystyny Jamroz."
            ]),
            City.Create("Końskie", Country.PL, [
                "Neoklasycystyczne oranżerie „Egipcjanki” w parku.",
                "Leży przy dawnej Kolei Iwangorodzko-Dąbrowskiej.",
                "Prawa miejskie od 1748 r."
            ]),
            City.Create("Jędrzejów", Country.PL, [
                "Najstarsze opactwo cysterskie w Polsce (1140 r.).",
                "Muzeum Przypkowskich – słynna kolekcja zegarów i gnomonów.",
                "Kolej wąskotorowa „Ciuchcia Express Ponidzie”."
            ]),
        
            City.Create("Warszawa", Country.PL, [
                "Stolica Polski od 1596 r.",
                "Stare Miasto odbudowane po II WŚ, na liście UNESCO (1980 r.).",
                "Największe centrum gospodarcze i kulturalne kraju."
            ]),
            City.Create("Kraków", Country.PL, [
                "Dawna stolica Polski do 1596 r.",
                "Rynek Główny – największy średniowieczny plac Europy.",
                "Wawel – miejsce koronacji i pochówku królów."
            ]),
            City.Create("Gdańsk", Country.PL, [
                "Kluczowy port bałtycki – członek Hanzy od XIII w.",
                "Strajki 1980 r. doprowadziły do powstania Solidarności.",
                "Żuraw i Długie Pobrzeże to wizytówki miasta."
            ]),
            City.Create("Wrocław", Country.PL, [
                "Miasto „100 mostów” nad Odrą i jej kanałami.",
                "Gotycki Ratusz na Rynku – jeden z największych w Polsce.",
                "Europejska Stolica Kultury 2016 r."
            ]),
            City.Create("Poznań", Country.PL, [
                "Jeden z najstarszych grodów Polski – Ostrów Tumski (966 r.).",
                "Południowe koziołki Ratusza trykają się codziennie o 12:00.",
                "Międzynarodowe Targi Poznańskie od 1921 r."
            ]),
            City.Create("Łódź", Country.PL, [
                "Dawny „polski Manchester” – centrum przemysłu włókienniczego.",
                "Ulica Piotrkowska ma ponad 4 km długości.",
                "Miasto filmu w Sieci UNESCO (od 2017 r.)."
            ]),
            City.Create("Szczecin", Country.PL, [
                "Port morski nad Odrą – 65 km od Bałtyku.",
                "Zamek Książąt Pomorskich – siedziba rodu Gryfitów.",
                "Układ ulic wzorowany na planie Paryża (Haussmann)."
            ]),
            City.Create("Białystok", Country.PL, [
                "Pałac Branickich – „Wersal Północy”.",
                "Miasto wielokulturowe; tu urodził się twórca Esperanto – L. Zamenhof.",
                "Brama do Puszczy Knyszyńskiej i Narwiańskiego PN."
            ]),
            City.Create("Katowice", Country.PL, [
                "Centrum 2-mln metropolii GZM.",
                "Hala „Spodek” – ikona architektury lat 70.",
                "Strefa Kultury powstała w miejscu dawnej kopalni „Katowice”."
            ]),
            City.Create("Lublin", Country.PL, [
                "Unia Lubelska 1569 r. połączyła Koronę i Litwę.",
                "Brama Krakowska jest symbolem miasta.",
                "Lubelskie podziemia prowadzą pod Starym Miastem."
            ])
        ];

        foreach (var city in cities)
        {
            if (!await context.Cities.AnyAsync(c => c.Name == city.Name, cancellationToken))
            {
                context.Cities.Add(city);
            }
        }
        
        await context.SaveChangesAsync(cancellationToken);
    }

    private static async Task SeedStations(TrackEasyDbContext context, CancellationToken cancellationToken)
    {
        var cities = await context.Cities.ToListAsync(cancellationToken);
        
        List<Station> stations =
        [
            Station.Create("Starachowice Wschodnie",  cities.Single(c => c.Name == "Starachowice"),          new GeographicalCoordinates(51.0416692m, 21.0789922m)), 
            Station.Create("Wąchock",                 cities.Single(c => c.Name == "Wąchock"),               new GeographicalCoordinates(51.0802903m, 21.0152553m)), 
            Station.Create("Skarżysko-Kamienna",      cities.Single(c => c.Name == "Skarżysko-Kamienna"),    new GeographicalCoordinates(51.1159431m, 20.8816053m)), 
            Station.Create("Ostrowiec Świętokrzyski", cities.Single(c => c.Name == "Ostrowiec Świętokrzyski"),new GeographicalCoordinates(50.933505m, 21.376488m)),  
            Station.Create("Kielce Główne",           cities.Single(c => c.Name == "Kielce"),                new GeographicalCoordinates(50.8742892m, 20.6178469m)), 
            Station.Create("Sandomierz",              cities.Single(c => c.Name == "Sandomierz"),            new GeographicalCoordinates(50.660249m, 21.764701m)),   
            Station.Create("Busko-Zdrój",             cities.Single(c => c.Name == "Busko-Zdrój"),           new GeographicalCoordinates(50.4617811m, 20.6858128m)), 
            Station.Create("Końskie",                 cities.Single(c => c.Name == "Końskie"),               new GeographicalCoordinates(51.1902639m, 20.4228822m)), 
            Station.Create("Jędrzejów",               cities.Single(c => c.Name == "Jędrzejów"),             new GeographicalCoordinates(50.6438889m, 20.2750000m)), 
            Station.Create("Warszawa Centralna",      cities.Single(c => c.Name == "Warszawa"),              new GeographicalCoordinates(52.228611m, 21.003056m)),   
            Station.Create("Kraków Główny",           cities.Single(c => c.Name == "Kraków"),                new GeographicalCoordinates(50.065556m, 19.947222m)),   
            Station.Create("Gdańsk Główny",           cities.Single(c => c.Name == "Gdańsk"),                new GeographicalCoordinates(54.3553942m, 18.6439281m)), 
            Station.Create("Wrocław Główny",          cities.Single(c => c.Name == "Wrocław"),               new GeographicalCoordinates(51.0989844m,17.0366461m)),  
            Station.Create("Poznań Główny",           cities.Single(c => c.Name == "Poznań"),                new GeographicalCoordinates(52.401667m, 16.911667m)),   
            Station.Create("Łódź Fabryczna",          cities.Single(c => c.Name == "Łódź"),                  new GeographicalCoordinates(51.7694772m,19.4700042m)),  
            Station.Create("Szczecin Główny",         cities.Single(c => c.Name == "Szczecin"),              new GeographicalCoordinates(53.41903m, 14.55201m)),     
            Station.Create("Białystok",               cities.Single(c => c.Name == "Białystok"),             new GeographicalCoordinates(53.1339108m,23.1359611m)),  
            Station.Create("Katowice",                cities.Single(c => c.Name == "Katowice"),              new GeographicalCoordinates(50.258333m, 19.0175m)),     
            Station.Create("Lublin Główny",           cities.Single(c => c.Name == "Lublin"),                new GeographicalCoordinates(51.2311725m,22.5689492m))   
        ];
        
        foreach (var station in stations)
        {
            if (!await context.Stations.AnyAsync(s => s.Name == station.Name && s.CityId == station.CityId, cancellationToken))
            {
                context.Stations.Add(station);
            }
        }
        
        await context.SaveChangesAsync(cancellationToken);
    }

    private static async Task SeedOperators(TrackEasyDbContext context, CancellationToken cancellationToken)
    {
        List<Operator> operators =
        [
            Operator.Create("PKP Intercity", "IC"),
            Operator.Create("Polregio", "PR")
        ];
        
        foreach (var op in operators)
        {
            if (!await context.Operators.AnyAsync(o => o.Name == op.Name, cancellationToken))
            {
                context.Operators.Add(op);
            }
        }
        
        await context.SaveChangesAsync(cancellationToken);
    }
    
    private static async Task SeedCoaches(TrackEasyDbContext context, CancellationToken cancellationToken)
    {
        var operators = await context.Operators
            .Include(x => x.Coaches)
            .Include(x => x.Trains)
            .AsSplitQuery()
            .ToListAsync(cancellationToken);

        foreach (var @operator in operators)
        {
            for (var i = 1; i <= 100; i++)
            {
                try
                {
                    @operator.AddCoach($"C{i}", Enumerable.Range(1, 100));
                }
                catch (Exception e)
                {
                    // ignored
                }
            }
        }
        
        await context.SaveChangesAsync(cancellationToken);
    }
    
    private static async Task SeedTrains(TrackEasyDbContext context, CancellationToken cancellationToken)
    {
        var operators = await context.Operators
            .Include(x => x.Coaches)
            .Include(x => x.Trains)
            .AsSplitQuery()
            .ToListAsync(cancellationToken);
        
        foreach (var @operator in operators)
        {
            for (var i = 0; i < 50; i++)
            {
                try
                {
                    @operator.AddTrain($"{@operator.Code}-{i+1}", [ 
                        (@operator.Coaches[i].Id, 1),
                        (@operator.Coaches[i+50].Id, 2)
                    ]);
                }
                catch (Exception e)
                {
                    // ignored
                }
            }
        }
        
        await context.SaveChangesAsync(cancellationToken);
    }
    
    private static async Task SeedConnections(TrackEasyDbContext context, CancellationToken cancellationToken)
    {
        var operators = await context.Operators
            .Include(o => o.Trains)
            .ToListAsync(cancellationToken);

        var stations = await context.Stations
            .ToDictionaryAsync(s => s.Name, cancellationToken);

        var allDays = new[]
        {
            DayOfWeek.Monday, DayOfWeek.Tuesday, DayOfWeek.Wednesday,
            DayOfWeek.Thursday, DayOfWeek.Friday, DayOfWeek.Saturday,
            DayOfWeek.Sunday
        };
        
        var ic = operators.Single(o => o.Code == "IC");
        var pr = operators.Single(o => o.Code == "PR");

        var connections = new List<Connection>
        {
            Connection.Create(
                name: "IC STAR-WAW",
                @operator: ic,
                pricePerKilometer: new Money(0.30m, Currency.PLN),
                train: ic.Trains[0],
                schedule: new Schedule(
                    new DateOnly(2025, 1, 1),
                    new DateOnly(2025, 12, 31),
                    allDays),
                stations: [
                    ConnectionStation.Create(stations["Starachowice Wschodnie"], null, new TimeOnly(6, 0), 1),
                    ConnectionStation.Create(stations["Kielce Główne"], new TimeOnly( 6,45), new TimeOnly(6,50), 2),
                    ConnectionStation.Create(stations["Warszawa Centralna"],  new TimeOnly( 8,50), null, 3)
                ],
                needsSeatReservation: true),

            Connection.Create(
                name: "IC WAW-KAT",
                @operator: ic,
                pricePerKilometer: new Money(0.30m, Currency.PLN),
                train: ic.Trains.Skip(1).First(),
                schedule: new Schedule(
                    new DateOnly(2025, 1, 1),
                    new DateOnly(2025, 12, 31),
                    allDays),
                stations: [
                    ConnectionStation.Create(stations["Warszawa Centralna"], null, new TimeOnly( 9,30), 1),
                    ConnectionStation.Create(stations["Kraków Główny"], new TimeOnly(12, 0), new TimeOnly(12, 5), 2),
                    ConnectionStation.Create(stations["Katowice"], new TimeOnly(12,55), null, 3)
                ],
                needsSeatReservation: true),

            Connection.Create(
                name: "IC KAT-SZN",
                @operator: ic,
                pricePerKilometer: new Money(0.30m, Currency.PLN),
                train: ic.Trains.Skip(2).First(),
                schedule: new Schedule(
                    new DateOnly(2025, 1, 1),
                    new DateOnly(2025, 12, 31),
                    allDays),
                stations: [
                    ConnectionStation.Create(stations["Katowice"], null, new TimeOnly(14,10), 1),
                    ConnectionStation.Create(stations["Poznań Główny"], new TimeOnly(17, 5), new TimeOnly(17,10), 2),
                    ConnectionStation.Create(stations["Szczecin Główny"], new TimeOnly(20,15), null, 3)
                ],
                needsSeatReservation: true),

            Connection.Create(
                name: "PR STAR-KIE",
                @operator: pr,
                pricePerKilometer: new Money(0.20m, Currency.PLN),
                train: pr.Trains.First(),
                schedule: new Schedule(
                    new DateOnly(2025, 1, 1),
                    new DateOnly(2025, 12, 31),
                    allDays),
                stations: [
                    ConnectionStation.Create(stations["Starachowice Wschodnie"], null, new TimeOnly( 7,15), 1),
                    ConnectionStation.Create(stations["Skarżysko-Kamienna"], new TimeOnly( 7,35), new TimeOnly( 7,40), 2),
                    ConnectionStation.Create(stations["Kielce Główne"], new TimeOnly( 8,15), null, 3)
                ],
                needsSeatReservation: false),

            Connection.Create(
                name: "PR KIE-BUS",
                @operator: pr,
                pricePerKilometer: new Money(0.20m, Currency.PLN),
                train: pr.Trains.Skip(1).First(),
                schedule: new Schedule(
                    new DateOnly(2025, 1, 1),
                    new DateOnly(2025, 12, 31),
                    allDays),
                stations: [
                    ConnectionStation.Create(stations["Kielce Główne"], null, new TimeOnly( 9, 0), 1),
                    ConnectionStation.Create(stations["Busko-Zdrój"], new TimeOnly(10,15), null, 2)
                ],
                needsSeatReservation: false),

            Connection.Create(
                name: "PR KIE-SAN",
                @operator: pr,
                pricePerKilometer: new Money(0.20m, Currency.PLN),
                train: pr.Trains.Skip(2).First(),
                schedule: new Schedule(
                    new DateOnly(2025, 1, 1),
                    new DateOnly(2025, 12, 31),
                    allDays),
                stations: [
                    ConnectionStation.Create(stations["Kielce Główne"], null, new TimeOnly(11,20), 1),
                    ConnectionStation.Create(stations["Jędrzejów"], new TimeOnly(11,55), new TimeOnly(12, 0), 2),
                    ConnectionStation.Create(stations["Sandomierz"], new TimeOnly(13,30), null,               3)
                ],
                needsSeatReservation: false)
        };

        foreach (var c in connections)
        {
            if (!await context.Connections.AnyAsync(x => x.Name == c.Name, cancellationToken))
            {
                c.ApproveRequest();
                context.Connections.Add(c);
            }
        }

        await context.SaveChangesAsync(cancellationToken);
    }
}