using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Grow.Server.Model.Entities;
using Grow.Server.Model.Entities.JoinEntities;
using Microsoft.EntityFrameworkCore;

namespace Grow.Server.Model.Utils
{
    public static class SeederFor2018Data
    {
        public static bool IsEnabled { get; set; }

        public static Person[] organizers { get; private set; }
        public static Person[] judges { get; private set; }
        public static Person[] mentors { get; private set; }
        public static Event[] events { get; private set; }
        public static Partner[] partners { get; private set; }
        public static Image[] images { get; private set; }
        public static Team[] teams { get; private set; }
        public static Contest[] contests { get; private set; }

        private static readonly IDictionary<Type, int> CurrentIds;


        static SeederFor2018Data()
        {
            IsEnabled = false;
            CurrentIds = new Dictionary<Type, int>();

            AddImages();
            AddEvents();
            AddPartners();
            AddPeople();
            AddTeams();
            AddContests();
        }


        public static void SeedDataFrom2018(this ModelBuilder modelBuilder)
        {
            if (!IsEnabled)
                return;

            modelBuilder.Entity<Image>().HasData(images);
            modelBuilder.Entity<Person>().HasData(mentors, judges, organizers);
            modelBuilder.Entity<Event>().HasData(events);
            modelBuilder.Entity<Partner>().HasData(partners);
            modelBuilder.Entity<Team>().HasData(teams);

            modelBuilder.Entity<Contest>().HasData(
            );
        }

        private static void AddPeople()
        {
            // Judges
            judges = new[] {
                new Person
                {
                    Id = NextId<Person>(),
                    Name = "Holger Kujath",
                    Description = "Founder and CEO of the online chat community Knuddels",
                    Image = null
                },
                new Person
                {
                    Id = NextId<Person>(),
                    Name = "Orestis Terzidis",
                    Description = "Professor and head of the entrepreneurial institute EnTechnon",
                    Image = null
                },
                new Person
                {
                    Id = NextId<Person>(),
                    Name = "Michael Kimmig",
                    Description = "Head of Corporate Process Management at GRENKE digital",
                    Image = null
                },
                new Person
                {
                    Id = NextId<Person>(),
                    Name = "Bernhard Janke",
                    Description = "Principal at the VC company LEA Partners",
                    Image = null
                },
                new Person
                {
                    Id = NextId<Person>(),
                    Name = "Martin Trenkle",
                    Description = "Founder and CEO of the job placement service Campusjäger",
                    Image = null
                },
                new Person
                {
                    Id = NextId<Person>(),
                    Name = "Daniel Stammler",
                    Description = "Co-Founder and Co-CEO of the mobile game company Kolibri Games",
                    Image = null
                }
            };

            // Organizers
            organizers = new[] {
                new Person
                {
                    Id = NextId<Person>(),
                    Name = "Dominik Doerner",
                    JobTitle = "Main Coordination",
                    Image = null
                },
                new Person
                {
                    Id = NextId<Person>(),
                    Name = "Anne-Cathrine Eimer",
                    JobTitle = "Workshop Program",
                    Image = null
                },
                new Person
                {
                    Id = NextId<Person>(),
                    Name = "Christian Wiegand",
                    JobTitle = "Fundraising",
                    Image = null
                },
                new Person
                {
                    Id = NextId<Person>(),
                    Name = "Jasmin Riedel",
                    JobTitle = "Event Management",
                    Image = null
                },
                new Person
                {
                    Id = NextId<Person>(),
                    Name = "Martin Thoma",
                    JobTitle = "Mentoring Program",
                    Image = null
                },
                new Person
                {
                    Id = NextId<Person>(),
                    Name = "Antonia Lorenz",
                    JobTitle = "Marketing",
                    Image = null
                }
            };

            // Mentors
            mentors = new[] {
                new Person
                {
                    Id = NextId<Person>(),
                    Name = "Sebastian Böhmer",
                    JobTitle = "Mentoring Program",
                    Expertise = "Venture capital, business development, finance, legal",
                    Description = null,
                    WebsiteUrl = "https://www.linkedin.com/in/sebastianboehmer/",
                    Image = null
                },
                new Person
                {
                    Id = NextId<Person>(),
                    Name = "Hans-Lothar Busch",
                    JobTitle = "Trainer & coach for the acquisition of industry projects",
                    Expertise = "Acquisition of industry projects, marketing, leadership, purchasing",
                    Description = null,
                    WebsiteUrl = "https://www.mehr-industrieprojekte.de/%C3%BCber-mich/",
                    Image = null
                },
                new Person
                {
                    Id = NextId<Person>(),
                    Name = "Murat Ercan",
                    JobTitle = "CEO at MEK Webdesign",
                    Expertise = "Online Marketing, Performance Marketing, Social Media, Webdesign and Sales ",
                    Description = null,
                    WebsiteUrl = null,
                    Image = null
                },
                new Person
                {
                    Id = NextId<Person>(),
                    Name = "Andreas Fischer",
                    JobTitle = "Founding Partner at First Momentum Ventures",
                    Expertise = "Idea validation, strategy, financing ",
                    Description = null,
                    WebsiteUrl = "https://www.linkedin.com/in/afischerfmv/",
                    Image = null
                },
                new Person
                {
                    Id = NextId<Person>(),
                    Name = "Jonas Fuchs",
                    JobTitle = "Founder & CEO at Usertimes Solution GmbH",
                    Expertise = "Student perspective, MVPs, networking, company culture, sales ",
                    Description = null,
                    WebsiteUrl = null,
                    Image = null
                },
                new Person
                {
                    Id = NextId<Person>(),
                    Name = "Peter Greiner",
                    JobTitle = "Active and financial investor for startups",
                    Expertise = "Business modelling, founding, organizational design, financing, sales ",
                    Description = null,
                    WebsiteUrl = "https://www.xing.com/profile/Peter_Greiner25",
                    Image = null
                },
                new Person
                {
                    Id = NextId<Person>(),
                    Name = "Cécile F. Heger",
                    JobTitle = "CEO at ABC-Vidal",
                    Expertise = "Finance, salaries, organization, strategy ",
                    Description = null,
                    WebsiteUrl = "https://www.linkedin.com/in/c%C3%A9cile-f-heger-1593a062",
                    Image = null
                },
                new Person
                {
                    Id = NextId<Person>(),
                    Name = "Manuel Köcher",
                    JobTitle = "Manager at the CIE (KIT) and business consultant",
                    Expertise = "Organizational design, market analysis, product development, strategy ",
                    Description = null,
                    WebsiteUrl = null,
                    Image = null
                },
                new Person
                {
                    Id = NextId<Person>(),
                    Name = "Karl Lorey",
                    JobTitle = "Founding Partner at First Momentum Ventures",
                    Expertise = "Scraping, machine learning, MVP, web development, financing ",
                    Description = null,
                    WebsiteUrl = "https://www.linkedin.com/in/karllorey/",
                    Image = null
                },
                new Person
                {
                    Id = NextId<Person>(),
                    Name = "Maja Malovic",
                    JobTitle = "Business Innovation Manager and Design Thinking Coach",
                    Expertise = "Business modelling, design thinking, client communication, prototyping",
                    Description = null,
                    WebsiteUrl = "https://www.linkedin.com/in/maja-malovic-a5095b163/",
                    Image = null
                },
                new Person
                {
                    Id = NextId<Person>(),
                    Name = "Jannik Nefferdorf",
                    JobTitle = "Student and entrepeneurial enthusiast",
                    Expertise = "Startup ecosystem, business modelling, entrepeneurship basics ",
                    Description = null,
                    WebsiteUrl = "https://www.linkedin.com/in/neffi97/",
                    Image = null
                },
                new Person
                {
                    Id = NextId<Person>(),
                    Name = "Martin Rammensee",
                    JobTitle = "Founder & CEO at Green Parrot GmbH",
                    Expertise = "Strategy, market entry, mobility, B2B ",
                    Description = null,
                    WebsiteUrl = "https://www.linkedin.com/in/martin-rammensee-a7860398/",
                    Image = null
                },
                new Person
                {
                    Id = NextId<Person>(),
                    Name = "Nestor Rodriguez",
                    JobTitle = "Innovational tourist",
                    Expertise = "Market, startup scene, business development, inspiration ",
                    Description = null,
                    WebsiteUrl = "https://www.xing.com/profile/Nestor_Rodriguez",
                    Image = null
                },
                new Person
                {
                    Id = NextId<Person>(),
                    Name = "Ben Romberg",
                    JobTitle = "Founder of codefortynine GmbH",
                    Expertise = "Bootstrapping, lean startup, continuous delivery, business modelling, Y Combinator \"philosophy\"",
                    Description = null,
                    WebsiteUrl = "https://www.xing.com/profile/Ben_Romberg",
                    Image = null
                },
                new Person
                {
                    Id = NextId<Person>(),
                    Name = "Heinz T. Rothermel",
                    JobTitle = "Business consultant and lecturer",
                    Expertise = "Business planning, strategy, venture capital, marketing, communication ",
                    Description = null,
                    WebsiteUrl = "https://www.xing.com/profile/Heinz_Rothermel/",
                    Image = null
                },
                new Person
                {
                    Id = NextId<Person>(),
                    Name = "Frederic Tausch",
                    JobTitle = "CTO & Co-Founder at apic.ai",
                    Expertise = "AI, software development, bootstrapping, founding, student perspective ",
                    Description = null,
                    WebsiteUrl = "https://www.linkedin.com/in/frederic-tausch/",
                    Image = null
                }
            };
        }

        private static void AddPartners()
        {
            partners = new[] {
                new Partner
                {
                    Id = NextId<Partner>(),
                    Name = "Knuddels",
                    Description = "Online chat community",
                    Image = null
                },
                new Partner
                {
                    Id = NextId<Partner>(),
                    Name = "LEA Partners",
                    Description = "Local VC company",
                    Image = null
                },
                new Partner
                {
                    Id = NextId<Partner>(),
                    Name = "GRENKE Digital",
                    Description = "Financial service provider",
                    Image = null
                },
                new Partner
                {
                    Id = NextId<Partner>(),
                    Name = "Karlshochschule",
                    Description = "Private international university",
                    Image = null
                },
                new Partner
                {
                    Id = NextId<Partner>(),
                    Name = "First Momentum Ventures",
                    Description = "VC company founded by students",
                    Image = null
                },
                new Partner
                {
                    Id = NextId<Partner>(),
                    Name = "KIT Gründerschmiede",
                    Description = "Project of the KIT supporting R2B",
                    Image = null
                },
                new Partner
                {
                    Id = NextId<Partner>(),
                    Name = "EnTechnon",
                    Description = "Entrepreneurial institute at the KIT",
                    Image = null
                },
                new Partner
                {
                    Id = NextId<Partner>(),
                    Name = "Kolibri Games",
                    Description = "Mobile game developer",
                    Image = null
                }
            };
        }

        private static void AddEvents()
        {
            events = new[] {
                new Event
                {
                    Id = NextId<Event>(),
                    Name = "Kickoff",
                    Address = "Karlstraße 36 - 38, 76133 Karlsruhe",
                    Location = "Karlshochschule",
                    Description = "The kickoff is where the fun starts, no matter whether you have already applied or whether you're still undecided. We will present everything you need to know about the contest and give you a chance to find an idea and/or teammates. And the 11 weeks of work will start right away.",
                    FacebookLink = "https://www.facebook.com/events/328499797707535/",
                    Start = new DateTime(2018, 11, 12, 18, 0, 0).ToUniversalTime(),
                    End = new DateTime(2018, 11, 12, 21, 0, 0).ToUniversalTime(),
                    HasTimesSet = true,
                    IsMandatory = true,
                    HeldBy = null,
                    Visibility = Event.EventVisibility.Public,
                    Type = Event.EventType.MainEvent
                },
                new Event
                {
                    Id = NextId<Event>(),
                    Name = "Seed Day",
                    Address = "Rintheimer Str. 15, 76131 Karlsruhe",
                    Location = "PionierGarage Launchpad",
                    Description = "At the seed day we want to connect teams and mentors. First, you get a short chance to pitch both your idea and what you need help with in front of everyone. Then you can find the most suitable mentors in short one-on-one talks. ",
                    FacebookLink = null,
                    Start = new DateTime(2018, 11, 19, 19, 0, 0).ToUniversalTime(),
                    End = new DateTime(2018, 11, 19, 22, 0, 0).ToUniversalTime(),
                    HasTimesSet = true,
                    IsMandatory = false,
                    HeldBy = null,
                    Visibility = Event.EventVisibility.ForAllTeams,
                    Type = Event.EventType.Mentoring
                },
                new Event
                {
                    Id = NextId<Event>(),
                    Name = "Workshop \"Innovation\"",
                    Address = "Rintheimer Str. 15, 76131 Karlsruhe",
                    Location = "PionierGarage Launchpad",
                    Description = "It\'s important to understand your customers - observe, ask the right questions and structure information.\r\nIn this workshop you will learn both theoretical principles and practical methods such as Design Thinking and Jobs-To-Be-Done that can be applied to improve your own business model right away.\r\n\r\nHeld by the EnTechnon, the entrepreneurship institute at the KIT, in the format of their own upCat accelerator.",
                    FacebookLink = null,
                    Start = new DateTime(2018, 11, 20, 9, 30, 0).ToUniversalTime(),
                    End = new DateTime(2018, 11, 20, 16, 30, 0).ToUniversalTime(),
                    HasTimesSet = true,
                    IsMandatory = false,
                    HeldBy = null,
                    Visibility = Event.EventVisibility.ForAllTeams,
                    Type = Event.EventType.Workshop
                },
                new Event
                {
                    Id = NextId<Event>(),
                    Name = "Workshop \"Business Modeling\"",
                    Address = "Rintheimer Str. 15, 76131 Karlsruhe",
                    Location = "PionierGarage Launchpad",
                    Description = "To execute your own business means to understand who you are and what you do.\r\nIn this workshop you will use multiple methods and formats to display all relevant activities and aspects of your venture. With differing levels of abstraction those help you to understand the relationship between your processes and guide you through the phases of ideation, feasability, prototyping and decision-making.\r\n\r\nHeld by the EnTechnon, the entrepreneurship institute at the KIT, in the format of their own upCat accelerator.",
                    FacebookLink = null,
                    Start = new DateTime(2018, 11, 21, 9, 30, 0).ToUniversalTime(),
                    End = new DateTime(2018, 11, 21, 16, 30, 0).ToUniversalTime(),
                    HasTimesSet = true,
                    IsMandatory = false,
                    HeldBy = null,
                    Visibility = Event.EventVisibility.ForAllTeams,
                    Type = Event.EventType.Workshop
                },
                new Event
                {
                    Id = NextId<Event>(),
                    Name = "Workshop \"Pitching\"",
                    Address = "Rintheimer Str. 15, 76131 Karlsruhe",
                    Location = "PionierGarage Launchpad",
                    Description = "So much to say and so little time to do so.\r\nPitching your project requires you to be authentic, deliver a lot of information, help your audience to understand you emotionally and keep listeners engaged. This workshop aims to help you persuade both potential customers as well as judges and investors and improve your pitch-deck.\r\n\r\nHeld by the EnTechnon, the entrepreneurship institute at the KIT, in the format of their own upCat accelerator.",
                    FacebookLink = null,
                    Start = new DateTime(2018, 12, 05, 9, 30, 0).ToUniversalTime(),
                    End = new DateTime(2018, 12, 05, 16, 30, 0).ToUniversalTime(),
                    HasTimesSet = true,
                    IsMandatory = false,
                    HeldBy = null,
                    Visibility = Event.EventVisibility.ForAllTeams,
                    Type = Event.EventType.Workshop
                },
                new Event
                {
                    Id = NextId<Event>(),
                    Name = "Midterm",
                    Address = "Kaiserstraße 146, 76133 Karlsruhe",
                    Location = "Knuddels Office",
                    Description = "Half time break! Pitch what you've done in the last 5 weeks in front of a small audience and a jury. The judges will select 10 teams out of all participants to continue with the second half of the content and ultimately the final.",
                    FacebookLink = "https://www.facebook.com/events/592799921164435/",
                    Start = new DateTime(2018, 12, 09, 19, 0, 0).ToUniversalTime(),
                    End = new DateTime(2018, 12, 09, 22, 0, 0).ToUniversalTime(),
                    HasTimesSet = true,
                    IsMandatory = true,
                    HeldBy = null,
                    Visibility = Event.EventVisibility.Public,
                    Type = Event.EventType.MainEvent
                },
                new Event
                {
                    Id = NextId<Event>(),
                    Name = "Workshop \"Financing\"",
                    Address = "Rintheimer Str. 15, 76131 Karlsruhe",
                    Location = "PionierGarage Launchpad",
                    Description = "Financing is about getting the funds necessary to build your idea and/or project, especially using investment money. And who would be better suited to tutor you than the two Karlsruhe-based Venture Capital companies LEA Partners and First Momentum Ventures.",
                    FacebookLink = null,
                    Start = new DateTime(2019, 01, 10, 09, 30, 0).ToUniversalTime(),
                    End = new DateTime(2019, 01, 10, 13, 0, 0).ToUniversalTime(),
                    HasTimesSet = true,
                    IsMandatory = false,
                    HeldBy = null,
                    Visibility = Event.EventVisibility.ForAllTeams,
                    Type = Event.EventType.Workshop
                },
                new Event
                {
                    Id = NextId<Event>(),
                    Name = "Final",
                    Address = "Building 30.95, Karlsruhe Institute of Technology, 76131 Karlsruhe",
                    Location = "Audimax",
                    Description = "This is what you\'ve been working for!\r\nPitch one last time in front of a huge audience and show what you\'ve learned and how far you have come. Every pitch is followed by a short round of questions by the judges before you can get a chance at winning one of the prizes. ",
                    FacebookLink = "https://www.facebook.com/events/288450928456010/",
                    Start = new DateTime(2019, 01, 30, 19, 0, 0).ToUniversalTime(),
                    End = new DateTime(2019, 01, 30, 22, 0, 0).ToUniversalTime(),
                    HasTimesSet = true,
                    IsMandatory = true,
                    HeldBy = null,
                    Visibility = Event.EventVisibility.Public,
                    Type = Event.EventType.MainEvent
                }
            };
        }

        private static void AddImages()
        {

        }

        private static void AddTeams()
        {
            teams = new[]
            {
                new Team
                {
                    Id = NextId<Team>(),
                    TeamName = "Bavest",
                    ActiveSince = "Spring 2018",
                    Description = "Bavest ist ein Fintech, also ein Unternehmen das technologische Finanzinnovationen anbietet, die auf neusten Technologien basieren. Bavest sammelt Daten mit Hilfe von Data Crawlern und APIs. Die Daten werden mit künstlicher Intelligenz (Machine Learning im Fall von Horizon) analysiert. Maschinelles Lernen ist ein Oberbegriff für die „künstliche“ Generierung von Wissen aus Erfahrung. Durch die intelligenten Algorithmen und deren Analyse kann Horizon, unsere erste Innovation, Fundamentaldaten analysieren und intelligente Investmentstrategien darlegen. Später möchten wir bei Bavest weitere intelligente Produkte anbieten, die das Investieren vereinfachen. Dabei wollen wir Produkte schaffen, die es ermöglichen, Anlegern einen guten Einblick zu geben, unabhängig von Banken (Bank Beratern) oder anderen Institutionen. Dabei sollen unsere Kunden auf möglichst neueste Technologien Zugriff haben, wie z.B Machine Learning etc.",
                    Email = "support@bavest.org",
                    WebsiteUrl = "https://www.bavest.org/",
                    FacebookUrl = "https://www.facebook.com/BavestDE",
                    InstagramUrl = null,
                    LogoImage = null,
                    TeamPhoto = null,
                    MembersAsString = "Ramtin Babaei, Pedram Babaei",
                    IsActive = true
                },
            };
        }

        private static void AddContests()
        {
            var contest = new Contest
            {
                Id = NextId<Contest>(),
                Name = "GROW 2018/19",
                Language = "English",
                Teams = teams,
                Events = events,
                KickoffEvent = events.First(),
                FinalEvent = events.Last()
            };

            contest.Mentors = TransformToJoinEntities<MentorToContest>(contest, mentors);
            contest.Judges = TransformToJoinEntities<JudgeToContest>(contest, judges);
            contest.Organizers = TransformToJoinEntities<OrganizerToContest>(contest, organizers);
            contest.Partners = TransformToJoinEntities<PartnerToContest>(contest, partners);

            contests = new[] { contest };
        }


        private static TJoin[] TransformToJoinEntities<TJoin>(Contest contest, Person[] people) where TJoin : PersonToContest, new()
        {
            var joins = new TJoin[people.Length];
            for (var i = 0; i < people.Length; i++)
            {
                var person = people[i];
                var join = new TJoin
                {
                    Contest = contest,
                    ContestId = contest.Id,
                    Person = person,
                    PersonId = person.Id
                };

                joins[i] = join;
            }
            return joins;
        }

        private static TJoin[] TransformToJoinEntities<TJoin>(Contest contest, Partner[] partners) where TJoin : PartnerToContest, new()
        {
            var joins = new TJoin[partners.Length];
            for (var i = 0; i < partners.Length; i++)
            {
                var partner = partners[i];
                var join = new TJoin
                {
                    Contest = contest,
                    ContestId = contest.Id,
                    Partner = partner,
                    PartnerId = partner.Id
                };

                joins[i] = join;
            }
            return joins;
        }

        private static int NextId<TType>()
        {
            if (!CurrentIds.ContainsKey(typeof(TType)))
                CurrentIds.Add(typeof(TType), 0);

            CurrentIds[typeof(TType)] = CurrentIds[typeof(TType)] + 1;

            return CurrentIds[typeof(TType)];
        }
    }
}
