using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Grow.Server.Model.Entities;
using Grow.Server.Model.Entities.JoinEntities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore.Migrations;

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
        public static Image[] orga_images { get; private set; }
        public static Image[] judge_images { get; private set; }
        public static Image[] mentor_images { get; private set; }
        public static Image[] team_images { get; private set; }
        public static Image[] partner_images { get; private set; }
        public static Team[] teams { get; private set; }
        public static Prize[] prizes { get; private set; }

        private static readonly IDictionary<Type, int> CurrentIds;


        static SeederFor2018Data()
        {
            IsEnabled = true;
            CurrentIds = new Dictionary<Type, int>();

            AddImages();
            AddEvents();
            AddPartners();
            AddPeople();
            AddTeams();
            AddPrizes();
        }

        
        public static void ResetDatabase(this GrowDbContext context)
        {
            context.GetService<IMigrator>().Migrate(Migration.InitialDatabase);
            context.Database.Migrate();
        }

        public static void SeedDataFrom2018(this GrowDbContext context)
        {
            if (!IsEnabled)
                return;

            if (context.Images.Any())
            {
                Debug.WriteLine("Could not seed database - database is not empty");
                return;
            }

            // Add pre-defined data
            context.Images.AddRange(judge_images);
            context.Images.AddRange(mentor_images);
            context.Images.AddRange(team_images);
            context.Images.AddRange(orga_images);
            context.Images.AddRange(partner_images);

            context.Persons.AddRange(mentors);
            context.Persons.AddRange(judges);
            context.Persons.AddRange(organizers);

            context.Events.AddRange(events);
            context.Partners.AddRange(partners);
            context.Teams.AddRange(teams);
            context.Prizes.AddRange(prizes);

            context.SaveChanges();

            // Add contest
            var contest = new Contest
            {
                Name = "GROW 2018/19",
                Language = "English",
                Teams = teams,
                Prizes = prizes
            };

            // Set special collection navigation properties
            contest.Events = AddReferencesInCollection(contest, events, (e, c) => { e.Contest = c; });
            contest.Mentors = TransformToJoinEntities<MentorToContest>(contest, mentors);
            contest.Judges = TransformToJoinEntities<JudgeToContest>(contest, judges);
            contest.Organizers = TransformToJoinEntities<OrganizerToContest>(contest, organizers);
            contest.Partners = TransformToJoinEntities<PartnerToContest>(contest, partners);
            
            context.SaveChanges();

            // Special additional properties to avoid circular references
            contest.KickoffEvent = contest.Events.First();
            contest.FinalEvent = contest.Events.Last();

            context.SaveChanges();
        }


        private static void AddPeople()
        {
            // Judges
            judges = new[] {
                new Person
                {
                    Name = "Holger Kujath",
                    Description = "Founder and CEO of the online chat community Knuddels",
                    Image = judge_images[2]
                },
                new Person
                {
                    Name = "Orestis Terzidis",
                    Description = "Professor and head of the entrepreneurial institute EnTechnon",
                    Image = judge_images[5]
                },
                new Person
                {
                    Name = "Michael Kimmig",
                    Description = "Head of Corporate Process Management at GRENKE digital",
                    Image = judge_images[4]
                },
                new Person
                {
                    Name = "Bernhard Janke",
                    Description = "Principal at the VC company LEA Partners",
                    Image = judge_images[0]
                },
                new Person
                {
                    Name = "Martin Trenkle",
                    Description = "Founder and CEO of the job placement service Campusjäger",
                    Image = judge_images[3]
                },
                new Person
                {
                    Name = "Daniel Stammler",
                    Description = "Co-Founder and Co-CEO of the mobile game company Kolibri Games",
                    Image = judge_images[1]
                }
            };

            // Organizers
            organizers = new[] {
                new Person
                {
                    Name = "Dominik Doerner",
                    JobTitle = "Main Coordination",
                    Image = orga_images[3]
                },
                new Person
                {
                    Name = "Anne-Cathrine Eimer",
                    JobTitle = "Workshop Program",
                    Image = orga_images[0]
                },
                new Person
                {
                    Name = "Christian Wiegand",
                    JobTitle = "Fundraising",
                    Image = orga_images[2]
                },
                new Person
                {
                    Name = "Jasmin Riedel",
                    JobTitle = "Event Management",
                    Image = orga_images[4]
                },
                new Person
                {
                    Name = "Martin Thoma",
                    JobTitle = "Mentoring Program",
                    Image = orga_images[5]
                },
                new Person
                {
                    Name = "Antonia Lorenz",
                    JobTitle = "Marketing",
                    Image = orga_images[1]
                }
            };

            // Mentors
            mentors = new[] {
                new Person
                {
                    Name = "Sebastian Böhmer",
                    JobTitle = "Founding Partner at First Momentum Ventures",
                    Expertise = "Venture capital, business development, finance, legal",
                    Description = null,
                    WebsiteUrl = "https://www.linkedin.com/in/sebastianboehmer/",
                    Image = mentor_images[15]
                },
                new Person
                {
                    Name = "Hans-Lothar Busch",
                    JobTitle = "Trainer & coach for the acquisition of industry projects",
                    Expertise = "Acquisition of industry projects, marketing, leadership, purchasing",
                    Description = null,
                    WebsiteUrl = "https://www.mehr-industrieprojekte.de/%C3%BCber-mich/",
                    Image = mentor_images[4]
                },
                new Person
                {
                    Name = "Murat Ercan",
                    JobTitle = "CEO at MEK Webdesign",
                    Expertise = "Online Marketing, Performance Marketing, Social Media, Webdesign and Sales ",
                    Description = null,
                    WebsiteUrl = null,
                    Image = mentor_images[12]
                },
                new Person
                {
                    Name = "Andreas Fischer",
                    JobTitle = "Founding Partner at First Momentum Ventures",
                    Expertise = "Idea validation, strategy, financing ",
                    Description = null,
                    WebsiteUrl = "https://www.linkedin.com/in/afischerfmv/",
                    Image = mentor_images[0]
                },
                new Person
                {
                    Name = "Jonas Fuchs",
                    JobTitle = "Founder & CEO at Usertimes Solution GmbH",
                    Expertise = "Student perspective, MVPs, networking, company culture, sales ",
                    Description = null,
                    WebsiteUrl = null,
                    Image = mentor_images[7]
                },
                new Person
                {
                    Name = "Peter Greiner",
                    JobTitle = "Active and financial investor for startups",
                    Expertise = "Business modelling, founding, organizational design, financing, sales ",
                    Description = null,
                    WebsiteUrl = "https://www.xing.com/profile/Peter_Greiner25",
                    Image = mentor_images[14]
                },
                new Person
                {
                    Name = "Cécile F. Heger",
                    JobTitle = "CEO at ABC-Vidal",
                    Expertise = "Finance, salaries, organization, strategy ",
                    Description = null,
                    WebsiteUrl = "https://www.linkedin.com/in/c%C3%A9cile-f-heger-1593a062",
                    Image = mentor_images[2]
                },
                new Person
                {
                    Name = "Manuel Köcher",
                    JobTitle = "Manager at the CIE (KIT) and business consultant",
                    Expertise = "Organizational design, market analysis, product development, strategy ",
                    Description = null,
                    WebsiteUrl = null,
                    Image = mentor_images[10]
                },
                new Person
                {
                    Name = "Karl Lorey",
                    JobTitle = "Founding Partner at First Momentum Ventures",
                    Expertise = "Scraping, machine learning, MVP, web development, financing ",
                    Description = null,
                    WebsiteUrl = "https://www.linkedin.com/in/karllorey/",
                    Image = mentor_images[8]
                },
                new Person
                {
                    Name = "Maja Malovic",
                    JobTitle = "Business Innovation Manager and Design Thinking Coach",
                    Expertise = "Business modelling, design thinking, client communication, prototyping",
                    Description = null,
                    WebsiteUrl = "https://www.linkedin.com/in/maja-malovic-a5095b163/",
                    Image = mentor_images[9]
                },
                new Person
                {
                    Name = "Jannik Nefferdorf",
                    JobTitle = "Student and entrepeneurial enthusiast",
                    Expertise = "Startup ecosystem, business modelling, entrepeneurship basics ",
                    Description = null,
                    WebsiteUrl = "https://www.linkedin.com/in/neffi97/",
                    Image = mentor_images[6]
                },
                new Person
                {
                    Name = "Martin Rammensee",
                    JobTitle = "Founder & CEO at Green Parrot GmbH",
                    Expertise = "Strategy, market entry, mobility, B2B ",
                    Description = null,
                    WebsiteUrl = "https://www.linkedin.com/in/martin-rammensee-a7860398/",
                    Image = mentor_images[11]
                },
                new Person
                {
                    Name = "Nestor Rodriguez",
                    JobTitle = "Innovational tourist",
                    Expertise = "Market, startup scene, business development, inspiration ",
                    Description = null,
                    WebsiteUrl = "https://www.xing.com/profile/Nestor_Rodriguez",
                    Image = mentor_images[13]
                },
                new Person
                {
                    Name = "Ben Romberg",
                    JobTitle = "Founder of codefortynine GmbH",
                    Expertise = "Bootstrapping, lean startup, continuous delivery, business modelling, Y Combinator \"philosophy\"",
                    Description = null,
                    WebsiteUrl = "https://www.xing.com/profile/Ben_Romberg",
                    Image = mentor_images[1]
                },
                new Person
                {
                    Name = "Heinz T. Rothermel",
                    JobTitle = "Business consultant and lecturer",
                    Expertise = "Business planning, strategy, venture capital, marketing, communication ",
                    Description = null,
                    WebsiteUrl = "https://www.xing.com/profile/Heinz_Rothermel/",
                    Image = mentor_images[5]
                },
                new Person
                {
                    Name = "Frederic Tausch",
                    JobTitle = "CTO & Co-Founder at apic.ai",
                    Expertise = "AI, software development, bootstrapping, founding, student perspective ",
                    Description = null,
                    WebsiteUrl = "https://www.linkedin.com/in/frederic-tausch/",
                    Image = mentor_images[3]
                }
            };
        }

        private static void AddPartners()
        {
            partners = new[] {
                new Partner
                {
                    Name = "Knuddels",
                    Description = "Online chat community",
                    Image = partner_images[9]
                },
                new Partner
                {
                    Name = "LEA Partners",
                    Description = "Local VC company",
                    Image = partner_images[7]
                },
                new Partner
                {
                    Name = "GRENKE Digital",
                    Description = "Financial service provider",
                    Image = partner_images[2]
                },
                new Partner
                {
                    Name = "Karlshochschule",
                    Description = "Private international university",
                    Image = partner_images[4]
                },
                new Partner
                {
                    Name = "First Momentum Ventures",
                    Description = "VC company founded by students",
                    Image = partner_images[1]
                },
                new Partner
                {
                    Name = "KIT Gründerschmiede",
                    Description = "Project of the KIT supporting R2B",
                    Image = partner_images[5]
                },
                new Partner
                {
                    Name = "EnTechnon",
                    Description = "Entrepreneurial institute at the KIT",
                    Image = partner_images[0]
                },
                new Partner
                {
                    Name = "Kolibri Games",
                    Description = "Mobile game developer",
                    Image = partner_images[6]
                }
            };
        }

        private static void AddEvents()
        {
            events = new[] {
                new Event
                {
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
            judge_images = new[]
            {
                new Image
                {
                    Url = "/img/2018/jury/bernhard.jpg",
                    AltText = "The judge Bernhard Janke"
                },
                new Image
                {
                    Url = "/img/2018/jury/daniel.jpg",
                    AltText = "The judge Daniel Stammler"
                },
                new Image
                {
                    Url = "/img/2018/jury/holger.jpg",
                    AltText = "The judge Holger Kujath"
                },
                new Image
                {
                    Url = "/img/2018/jury/martin.jpg",
                    AltText = "The judge Martin Trenkle"
                },
                new Image
                {
                    Url = "/img/2018/jury/michael.jpg",
                    AltText = "The judge Michael Kimmig"
                },
                new Image
                {
                    Url = "/img/2018/jury/orestis.jpg",
                    AltText = "The judge Orestis Terzidis"
                },
            };

            mentor_images = new[]
            {
                new Image
                {
                    Url = "/img/2018/mentors/andreas_fischer.jpg",
                    AltText = "The mentor Andreas Fischer"
                },
                new Image
                {
                    Url = "/img/2018/mentors/ben_romberg.jpg",
                    AltText = "The mentor Ben Romberg"
                },
                new Image
                {
                    Url = "/img/2018/mentors/cecile_heger.jpg",
                    AltText = "The mentor Cecile Heger"
                },
                new Image
                {
                    Url = "/img/2018/mentors/frederic_tausch.jpg",
                    AltText = "The mentor Frederic Tausch"
                },
                new Image
                {
                    Url = "/img/2018/mentors/hans_busch.png",
                    AltText = "The mentor Hans Busch"
                },
                new Image
                {
                    Url = "/img/2018/mentors/heinz_rothermel.jpg",
                    AltText = "The mentor Heinz Rothermel"
                },
                new Image
                {
                    Url = "/img/2018/mentors/jannik_nefferdorf.jpg",
                    AltText = "The mentor Jannik Nefferdorf"
                },
                new Image
                {
                    Url = "/img/2018/mentors/jonas_fuchs.jpg",
                    AltText = "The mentor Jonas Fuchs"
                },
                new Image
                {
                    Url = "/img/2018/mentors/karl_lorey.jpg",
                    AltText = "The mentor Karl Lorey"
                },
                new Image
                {
                    Url = "/img/2018/mentors/maja_malovic.jpg",
                    AltText = "The mentor Maja Malovic"
                },
                new Image
                {
                    Url = "/img/2018/mentors/manuel_koecher.jpg",
                    AltText = "The mentor Manuel Köcher"
                },
                new Image
                {
                    Url = "/img/2018/mentors/martin_rammensee.jpg",
                    AltText = "The mentor Martin Rammensee"
                },
                new Image
                {
                    Url = "/img/2018/mentors/murat_ercan.jpg",
                    AltText = "The mentor Murat Ercan"
                },
                new Image
                {
                    Url = "/img/2018/mentors/nestor_rodriguez.jpg",
                    AltText = "The mentor Nestor Rodriguez"
                },
                new Image
                {
                    Url = "/img/2018/mentors/peter_greiner.jpg",
                    AltText = "The mentor Peter Greiner"
                },
                new Image
                {
                    Url = "/img/2018/mentors/sebastian_boehmer.jpg",
                    AltText = "The mentor Sebastian Böhmer"
                },
            };

            team_images = new[]
            {
                new Image
                {
                    Url = "/img/2018/teams/accesmed_team.jpg",
                    AltText = "Team photo of Acces Medecins"
                },
                new Image
                {
                    Url = "/img/2018/teams/accessmed.png",
                    AltText = "Logo of Acces Medecins"
                },
                new Image
                {
                    Url = "/img/2018/teams/allopi.png",
                    AltText = "Logo of AlloPI"
                },
                new Image
                {
                    Url = "/img/2018/teams/bavest.png",
                    AltText = "Logo of Bavest"
                },
                new Image
                {
                    Url = "/img/2018/teams/bavest_team.png",
                    AltText = "Team photo of Bavest"
                },
                new Image
                {
                    Url = "/img/2018/teams/circle.png",
                    AltText = "Logo of Circle"
                },
                new Image
                {
                    Url = "/img/2018/teams/circle_team.jpg",
                    AltText = "Team photo of Circle"
                },
                new Image
                {
                    Url = "/img/2018/teams/gimmickgott.png",
                    AltText = "Team photo of GimmickGott"
                },
                new Image
                {
                    Url = "/img/2018/teams/gimmickgott_logo.png",
                    AltText = "Logo of GimmickGott"
                },
                new Image
                {
                    Url = "/img/2018/teams/heliopas.svg",
                    AltText = "Logo of HelioPas AI"
                },
                new Image
                {
                    Url = "/img/2018/teams/heliopas_team.jpg",
                    AltText = "Team photo of HelioPas AI"
                },
                new Image
                {
                    Url = "/img/2018/teams/kbox.png",
                    AltText = "Logo of Kbox"
                },
                new Image
                {
                    Url = "/img/2018/teams/mangolearn.jpg",
                    AltText = "Team photo of MangoLearn"
                },
                new Image
                {
                    Url = "/img/2018/teams/mangolearn.png",
                    AltText = "Logo of MangoLearn"
                },
                new Image
                {
                    Url = "/img/2018/teams/read.png",
                    AltText = "Logo of Read!"
                },
                new Image
                {
                    Url = "/img/2018/teams/secureradiationlab.png",
                    AltText = "Logo of SecureRadiationLab"
                },
                new Image
                {
                    Url = "/img/2018/teams/studentenfutter.png",
                    AltText = "Logo of StudentenFutter"
                },
                new Image
                {
                    Url = "/img/2018/teams/studentenfutter_team.jpg",
                    AltText = "Team photo of Studentenfutter"
                },
                new Image
                {
                    Url = "/img/2018/teams/tortenglueck.png",
                    AltText = "Logo of Tortenglück"
                },
                new Image
                {
                    Url = "/img/2018/teams/wetakehealthcare_team.jpg",
                    AltText = "Team photo of WeTakeHealthCare"
                },
                new Image
                {
                    Url = "/img/2018/teams/wthc.png",
                    AltText = "Logo of WeTakeHealthCare"
                },
                new Image
                {
                    Url = "/img/2018/teams/zircle.png",
                    AltText = "Logo of Zircle"
                },
            };

            orga_images = new[]
            {
                new Image
                {
                    Url = "/img/2018/team/anne.jpg",
                    AltText = "The team member Anne Eimer"
                },
                new Image
                {
                    Url = "/img/2018/team/antonia.jpg",
                    AltText = "The team member Antonia Lorenz"
                },
                new Image
                {
                    Url = "/img/2018/team/chris.jpg",
                    AltText = "The team member Christian Wiegand"
                },
                new Image
                {
                    Url = "/img/2018/team/dominik.jpg",
                    AltText = "The team member Dominik Doerner"
                },
                new Image
                {
                    Url = "/img/2018/team/jasmin.jpg",
                    AltText = "The team member Jasmin Riedel"
                },
                new Image
                {
                    Url = "/img/2018/team/martin.jpg",
                    AltText = "The team member Martin Thoma"
                },
            };

            partner_images = new[]
            {
                new Image
                {
                    Url = "/img/2018/partner/entechnon.png",
                    AltText = "Logo of the EnTechnon"
                },
                new Image
                {
                    Url = "/img/2018/partner/fmvc.png",
                    AltText = "Logo of First Momentum ventures"
                },
                new Image
                {
                    Url = "/img/2018/partner/grenke.png",
                    AltText = "Logo of GRENKE"
                },
                new Image
                {
                    Url = "/img/2018/partner/gruenderwoche.png",
                    AltText = "Logo of the Deutsche Gründerwoche"
                },
                new Image
                {
                    Url = "/img/2018/partner/karlshochschule.png",
                    AltText = "Logo of the Karlshochschule"
                },
                new Image
                {
                    Url = "/img/2018/partner/KGS_transparent.png",
                    AltText = "Logo of the KIT Gründerschmiede"
                },
                new Image
                {
                    Url = "/img/2018/partner/kolibri.png",
                    AltText = "Logo of Kolibri Games"
                },
                new Image
                {
                    Url = "/img/2018/partner/lea_partners.png",
                    AltText = "Logo of LEA Partners"
                },
                new Image
                {
                    Url = "/img/2018/partner/logo_cyb.png",
                    AltText = "Logo of the CyberForum"
                },
                new Image
                {
                    Url = "/img/2018/partner/logo_knuddel_big.png",
                    AltText = "Logo of Knuddels"
                },
            };
        }

        private static void AddTeams()
        {
            teams = new[]
            {
                new Team
                {
                    Name = "3K",
                    ActiveSince = null,
                    Description = "A series of new-generation furniture, which focuses on fitness monitoring.One of series we first product is the intellectual toilet, which has a talent of stool test. ",
                    Email = null,
                    WebsiteUrl = null,
                    FacebookUrl = null,
                    InstagramUrl = null,
                    LogoImage = null,
                    TeamPhoto = null,
                    MembersAsString = "Zeming Leng",
                    IsActive = false
                },
                new Team
                {
                    Name = "Acces Medecins",
                    ActiveSince = "2017",
                    Description = "Acces Medecins is a website and application on which people who lives in medical deserts can have access to medical care via teleconsulations. It’s like a medical skype. The patient just have to connect himself on the app to contact their doctor or other health professionals. They will be able to see their medical datas and to share them with their doctor. Teleconsultation can for example save emergency services time and ressources by taking care of people who doesn’t have urgency case.This platform can also improve the patients quality of life with health and dietary advices. We can also imagine mental health support and consultations.My app will be available on app store and google play.This application concern all the european countries.  I want also use connected object to improve the results of the diagnostic.",
                    Email = "contactaccesmedecins@gmail.com",
                    WebsiteUrl = null,
                    FacebookUrl = null,
                    InstagramUrl = "accesmedecins",
                    LogoImage = team_images[1],
                    TeamPhoto = team_images[0],
                    MembersAsString = "Fatimata Toure, Amir Akbari",
                    IsActive = false
                },
                new Team
                {
                    Name = "alloPI",
                    ActiveSince = null,
                    Description = "Home automation reduces the resident’s workload of domestic work and improves life quality. However, the installation,  the maintenance and even the application of Smart Home devices are complex operations that require technical knowledge. However, especially people without technical knowledge or with physical impairments would benefit the most by automating their environment. Therefore, we integrate an adaptive and intuitive human-machine interface system into the Smart Home that exceeds the capabilities of common mobile apps. The system adapts to preferences and existing handicaps of the user by using user profiles. We provide counseling, installation and remote maintenance for Smart Homes. No maintenance, no limits – just act your way!",
                    Email = null,
                    WebsiteUrl = null,
                    FacebookUrl = null,
                    InstagramUrl = null,
                    LogoImage = team_images[2],
                    TeamPhoto = null,
                    MembersAsString = "Daniel David, Christian Fleiner, Arjun Rai Gupta, Marvin Okoh, Brian Sailer",
                    IsActive = false
                },
                new Team
                {
                    Name = "AR.K.I.T",
                    ActiveSince = "Oct 2018",
                    Description = "AR-App for Museum",
                    Email = null,
                    WebsiteUrl = null,
                    FacebookUrl = null,
                    InstagramUrl = null,
                    LogoImage = null,
                    TeamPhoto = null,
                    MembersAsString = "Walid Elleuch, Kutay Yüksel",
                    IsActive = false
                },
                new Team
                {
                    Name = "Bavest",
                    ActiveSince = "Spring 2018",
                    Description = "Bavest ist ein Fintech, also ein Unternehmen das technologische Finanzinnovationen anbietet, die auf neusten Technologien basieren. Bavest sammelt Daten mit Hilfe von Data Crawlern und APIs. Die Daten werden mit künstlicher Intelligenz (Machine Learning im Fall von Horizon) analysiert. Maschinelles Lernen ist ein Oberbegriff für die „künstliche“ Generierung von Wissen aus Erfahrung. Durch die intelligenten Algorithmen und deren Analyse kann Horizon, unsere erste Innovation, Fundamentaldaten analysieren und intelligente Investmentstrategien darlegen. Später möchten wir bei Bavest weitere intelligente Produkte anbieten, die das Investieren vereinfachen. Dabei wollen wir Produkte schaffen, die es ermöglichen, Anlegern einen guten Einblick zu geben, unabhängig von Banken (Bank Beratern) oder anderen Institutionen. Dabei sollen unsere Kunden auf möglichst neueste Technologien Zugriff haben, wie z.B Machine Learning etc.",
                    Email = "support@bavest.org",
                    WebsiteUrl = "https://www.bavest.org/",
                    FacebookUrl = "BavestDE",
                    InstagramUrl = "bavest.de",
                    LogoImage = team_images[3],
                    TeamPhoto = team_images[4],
                    MembersAsString = "Ramtin Babaei, Pedram Babaei",
                    IsActive = true
                },
                new Team
                {
                    Name = "can",
                    ActiveSince = "Oct 2018",
                    Description = "can - the health mediation, a mediation between customers and companies.",
                    Email = null,
                    WebsiteUrl = null,
                    FacebookUrl = null,
                    InstagramUrl = null,
                    LogoImage = null,
                    TeamPhoto = null,
                    MembersAsString = "Kai Can Bilbixun Avci",
                    IsActive = false
                },
                new Team
                {
                    Name = "Circle",
                    ActiveSince = "Oct 2018",
                    Description = "The vision behind Circle is to build a bridge between the digital and the real world. Nowadays it's very simple to communicate over the Internet and therefore technology has become a huge part of our lives.But did it ever struck your mind that all these digital relations we have built over time impact our social life on a huge scale?Loneliness caused by communication is what many of us experience daily. When did our smartphone replace the contact with a real human being, face to face?Circle wants to change that.Our goal is to build an App capable of connecting people to others around them that share the same spontaneous interests.Circle is an assistant to let you rediscover reality.",
                    Email = "team@circleco.de",
                    WebsiteUrl = null,
                    FacebookUrl = "Circle-350104552390103",
                    InstagramUrl = "circlemediaco",
                    LogoImage = team_images[5],
                    TeamPhoto = team_images[6],
                    MembersAsString = "Alexandre Lehr, Finn von Lauppert, Lukas Wipf, Kai Firschau",
                    IsActive = true
                },
                new Team
                {
                    Name = "No Name - [Design Thinking]",
                    ActiveSince = null,
                    Description = "Main question: How do companies develop strategies and how do they implement them into their organization?",
                    Email = null,
                    WebsiteUrl = null,
                    FacebookUrl = null,
                    InstagramUrl = null,
                    LogoImage = null,
                    TeamPhoto = null,
                    MembersAsString = "Julian Seidel, Christian Zimmermann",
                    IsActive = false
                },
                new Team
                {
                    Name = "DIGETECH",
                    ActiveSince = "Jul 18",
                    Description = "We aim to use virtual reality in health care. For this, we currently focus on two segments: 1) Nursing schools asked us if we could create a VR tool for them to teach the nursing students more uncommon cases and make those situations repeatable. 2) In general, our intention is to create VR solutions to optimize physiotherapy and to motivate patients to train more regularly in order to improve their healing process.",
                    Email = null,
                    WebsiteUrl = null,
                    FacebookUrl = null,
                    InstagramUrl = null,
                    LogoImage = null,
                    TeamPhoto = null,
                    MembersAsString = "Frederik Grösche, Tobias Buchwald",
                    IsActive = false
                },
                new Team
                {
                    Name = "EBARA",
                    ActiveSince = null,
                    Description = "Digitalisierung von Kleinunternehmen ",
                    Email = null,
                    WebsiteUrl = null,
                    FacebookUrl = null,
                    InstagramUrl = null,
                    LogoImage = null,
                    TeamPhoto = null,
                    MembersAsString = "Yanis Fallah, Marco Ezzy",
                    IsActive = false
                },
                new Team
                {
                    Name = "Eshr",
                    ActiveSince = null,
                    Description = "Wir schlagen ein Framework für ein dezentrales, kontinuierlich lernendes AIaaS-System vor. Um den Beitrag von technischem und wissenschaftlichen Know-How sowie der nötigen Rechenkraft anzuregen entwerfen wir einen auf kryptographischen Methoden basierenden Sozialvertag, der Beiträge zu dem Projekt mit veräußerlichen Rechten zur Nutzung des Systems kompensiert.",
                    Email = null,
                    WebsiteUrl = null,
                    FacebookUrl = null,
                    InstagramUrl = null,
                    LogoImage = null,
                    TeamPhoto = null,
                    MembersAsString = "Chris Hiatt, Eugen Götz, Matthias Schedel, Marin Vlastelica P.",
                    IsActive = false
                },
                new Team
                {
                    Name = "Falafel Fuego ",
                    ActiveSince = "Sep 18",
                    Description = "New food start up ... creative food with original taste",
                    Email = null,
                    WebsiteUrl = null,
                    FacebookUrl = null,
                    InstagramUrl = null,
                    LogoImage = null,
                    TeamPhoto = null,
                    MembersAsString = "Abdallatif Ali, Ihab Awad",
                    IsActive = false
                },
                new Team
                {
                    Name = "Gimmickgott",
                    ActiveSince = "2017",
                    Description = "Hey there! We are Gimmickgott! A young team of 2 professional magicians who's mission is to make magic better. Gimmickgott is not only for magicians but also for those who are interested and want to bring their message or product to the next level. One part of GG-Magic is the online shop where magicians can buy new and modern magic tricks created by our team. On the other hand we do Consulting for speakers,  magicians , CEOs and everyone who is on stage or want to make an impact on their clients. What does it mean? We construct the technique and methods behind magic ideas that someone has! Or we think about a way to make messages of companies and speakers connect with the audience through magic. Our goal is to change the art of magic!",
                    Email = "info@ggmagic.de",
                    WebsiteUrl = null,
                    FacebookUrl = "gimmickgott",
                    InstagramUrl = "gimmickgott",
                    LogoImage = team_images[8],
                    TeamPhoto = team_images[7],
                    MembersAsString = "Madou Mann, Daniel Hank, Eike Dahle ",
                    IsActive = true
                },
                new Team
                {
                    Name = "HANG",
                    ActiveSince = null,
                    Description = "HIFI SPEAKERS",
                    Email = null,
                    WebsiteUrl = null,
                    FacebookUrl = null,
                    InstagramUrl = null,
                    LogoImage = null,
                    TeamPhoto = null,
                    MembersAsString = "Giorgi Tsutskiridze",
                    IsActive = false
                },
                new Team
                {
                    Name = "HelioPas AI",
                    ActiveSince = "Mar 2018",
                    Description = "The increasing weather extremes such as droughts take its toll on farmers and lead to growing financial losses. Crop failure of 30% in Germany, in some regions 100%, due to drought in 2018. We collect worldwide many different data pools such as in-situ data from farmers, satellite data, soil moisture data, weather data and process, combine or refine these data to provide cutting-edge environmental insights for the risk modelling of a new drought insurance product. Only when data is fused and interpreted by artificial intelligence, drought can be automatically and objectively determined at a field-level - worldwide. ",
                    Email = "info@heliopas.com",
                    WebsiteUrl = "http://www.heliopas.com",
                    FacebookUrl = null,
                    InstagramUrl = null,
                    LogoImage = team_images[9],
                    TeamPhoto = team_images[10],
                    MembersAsString = "Ingmar Wolff, Benno Ommerborn, Vladyslav Shapran",
                    IsActive = true
                },
                new Team
                {
                    Name = "HIRSCH Clothing & Apparel",
                    ActiveSince = "Oct 2018",
                    Description = "Clothing and Apparel from the heart of the Black Forest. The company aims to engage the customer in the design process and provide an experience that goes beyond just buying the product. With ecommerce and serivce models in mind, the aim is to create business models that disrupt the outdated and wasteful nature of most brick-and-mortar clothing companies, all at a lower cost.",
                    Email = null,
                    WebsiteUrl = null,
                    FacebookUrl = null,
                    InstagramUrl = null,
                    LogoImage = null,
                    TeamPhoto = null,
                    MembersAsString = "Omar Abousena",
                    IsActive = false
                },
                new Team
                {
                    Name = "Jacks Handy",
                    ActiveSince = "2016",
                    Description = "Repair, just works! ",
                    Email = null,
                    WebsiteUrl = null,
                    FacebookUrl = null,
                    InstagramUrl = null,
                    LogoImage = null,
                    TeamPhoto = null,
                    MembersAsString = "Yekta Görkem Baysan",
                    IsActive = false
                },
                new Team
                {
                    Name = "Kbox",
                    ActiveSince = "Dec 2017",
                    Description = "Kbox is a multi-lingual collaborative e-learning platform. It has been launched with a focus to promote vernacular learning keeping in mind the global linguistic diversity. It's the first such MOOC platform of its kind to provide all in one services right from skilling, re-skilling, job mapping to peer connection forum.",
                    Email = null,
                    WebsiteUrl = null,
                    FacebookUrl = null,
                    InstagramUrl = null,
                    LogoImage = team_images[11],
                    TeamPhoto = null,
                    MembersAsString = "Saksham Gupta, Ishita Gupta",
                    IsActive = true
                },
                new Team
                {
                    Name = "No Name - [LED Lamps]",
                    ActiveSince = null,
                    Description = "LED lamp controlled by WIFI and Alexa",
                    Email = null,
                    WebsiteUrl = null,
                    FacebookUrl = null,
                    InstagramUrl = null,
                    LogoImage = null,
                    TeamPhoto = null,
                    MembersAsString = "Sebastian Braun",
                    IsActive = false
                },
                new Team
                {
                    Name = "MangoLearn",
                    ActiveSince = null,
                    Description = "We develop a mobile app which enables you to go through interactive courses on your phone in comfortable way. We provide a variety of topics which you can experience in a playful environment with your friends and other like-minded people.",
                    Email = "mail@mangolearn.com",
                    WebsiteUrl = "https://mangolearn.com/",
                    FacebookUrl = null,
                    InstagramUrl = null,
                    LogoImage = team_images[13],
                    TeamPhoto = team_images[12],
                    MembersAsString = "Danil Fedorovsky, Fabian Illner",
                    IsActive = true
                },
                new Team
                {
                    Name = "Move Lines",
                    ActiveSince = "Jun 18",
                    Description = "At the moment the research for interesting travel destinations that align with the personal taste takes a lot of effort. Browsing through different sources like TripAdvisor, Travelblogs and Travelguides can be a very time-consuming task. We provide a mobile app to discover interesting travel routes through people that share similar interests.",
                    Email = null,
                    WebsiteUrl = null,
                    FacebookUrl = null,
                    InstagramUrl = null,
                    LogoImage = null,
                    TeamPhoto = null,
                    MembersAsString = "Lukas Klinzing, Marcus Zanquila",
                    IsActive = false
                },
                new Team
                {
                    Name = "Project Hayek",
                    ActiveSince = "Nov 18",
                    Description = "A crowdfunding platform for small- and mid-scale businesses.",
                    Email = null,
                    WebsiteUrl = null,
                    FacebookUrl = null,
                    InstagramUrl = null,
                    LogoImage = null,
                    TeamPhoto = null,
                    MembersAsString = "Lennard Böhnke, Alexander Völker",
                    IsActive = false
                },
                new Team
                {
                    Name = "Read!",
                    ActiveSince = "Oct 2018",
                    Description = "Read! provides AR glasses for reading to make reading books more comfortable, better accessible and digitizing the reading experience.",
                    Email = null,
                    WebsiteUrl = null,
                    FacebookUrl = null,
                    InstagramUrl = null,
                    LogoImage = team_images[14],
                    TeamPhoto = null,
                    MembersAsString = "Reyhan Düzgün, David Puljiz",
                    IsActive = true
                },
                new Team
                {
                    Name = "Recipe Me",
                    ActiveSince = "Nov 18",
                    Description = "The Idea is a platform where members can share their own recipes and simply use any one of them for free. What we will offer is a service that will deliver the exact amounts of the ingredients of the recipe when someone makes an order on demand. We are also thinking of taking special orders for a larger group (15-20 people) that plans to cook together.",
                    Email = null,
                    WebsiteUrl = null,
                    FacebookUrl = null,
                    InstagramUrl = null,
                    LogoImage = null,
                    TeamPhoto = null,
                    MembersAsString = "Abdallah Alshanawani, Hanna Choi",
                    IsActive = false
                },
                new Team
                {
                    Name = "SecureRadiationLab",
                    ActiveSince = "Aug 18",
                    Description = "We want to found a company developing and producing tools for radiometric measurements. Our long term goal is to develop specialized measurement solutions for industrial partners but to get into this market we want to start simple by producing contamination monitors for emergency response teams. Therefore our unique selling point will be simplicity in use without reducing functionality and using state of the art technology.",
                    Email = null,
                    WebsiteUrl = null,
                    FacebookUrl = null,
                    InstagramUrl = null,
                    LogoImage = team_images[15],
                    TeamPhoto = null,
                    MembersAsString = "Aaron Griesbaum, Felix Stengel, Johannes Neumaier, Dominic Kis",
                    IsActive = true
                },
                new Team
                {
                    Name = "Studentenfutter",
                    ActiveSince = "Jan 18",
                    Description = "We believe that everyone benefits from a strong community. That's why we aim to bring people together on a daily basis. We want to integrate this thought into an already existing routine we all have: Having lunch. We establish an environment where almost everyone can afford having lunch in a restaurant and provide social functionalities which ease setting up an appointment. Throughout the project, we focus on students first and plan to expand our target group within three years to city-offices.We generate revenue by monthly fees for restaurants. These restaurants can promote their dishes on our platform and get insights into customer engagement and menu acceptance in order to improve their business.",
                    Email = "friends@studentenfutter-app.com",
                    WebsiteUrl = "http://www.studentenfutter-app.com",
                    FacebookUrl = "Studentenfutter-262434551132887",
                    InstagramUrl = null,
                    LogoImage = team_images[16],
                    TeamPhoto = team_images[17],
                    MembersAsString = "Giorgio Groß, Mustafa Cint, Kevin Steinbach, Fabian Wenzel",
                    IsActive = true
                },
                new Team
                {
                    Name = "Syncosync",
                    ActiveSince = "2016",
                    Description = "Backups für Jedermann ohne großen Aufwand mit relativ kurzer Wiederherstellungsdauer im worst-case",
                    Email = null,
                    WebsiteUrl = null,
                    FacebookUrl = null,
                    InstagramUrl = null,
                    LogoImage = null,
                    TeamPhoto = null,
                    MembersAsString = "Till Skrodzki, Stephan Skrodzki",
                    IsActive = false
                },
                new Team
                {
                    Name = "TortenGlück",
                    ActiveSince = "Nov 18",
                    Description = "Have you ever baked a layer-cake - with dogh, cream and decoration? Do you still stick with the ready-baking mixture-Brownies? We think that it shouldn't be that hard to create a torte and we guarantee you to succed baking your very first layer-cake by TortenGlück. TortenGlück is a contruction kit, containing everythink you need to start baking. We developed dry mixtures for dogh, stuffing and decoration in any possible flavor. Our Vision is that you can combine your very individual layer-cake and with a step-by-step instuction we guarantee your success to 100%. ",
                    Email = "e.goebel@t-online.de",
                    WebsiteUrl = null,
                    FacebookUrl = null,
                    InstagramUrl = null,
                    LogoImage = team_images[18],
                    TeamPhoto = null,
                    MembersAsString = "Elisabeth Goebel, Tobias Budig, Patrick Theobalt, Leander Märkisch",
                    IsActive = true
                },
                new Team
                {
                    Name = "udentme",
                    ActiveSince = "Nov 18",
                    Description = "At the moment identification process is done via Postident or Videoident. Our idea is to provide a solution for identification as an app, so that it is possible to do it in every location which is taking part as a identificator. The bigger the network of identificators the easier is the process.",
                    Email = null,
                    WebsiteUrl = null,
                    FacebookUrl = null,
                    InstagramUrl = null,
                    LogoImage = null,
                    TeamPhoto = null,
                    MembersAsString = "Dmitriy Shibayev, Ilya Shibayev, Burak Atay",
                    IsActive = false
                },
                new Team
                {
                    Name = "VallerVentures",
                    ActiveSince = null,
                    Description = "We create a venture capitalist whose investment decision-making is executed solely by algorithms. ",
                    Email = null,
                    WebsiteUrl = null,
                    FacebookUrl = null,
                    InstagramUrl = null,
                    LogoImage = null,
                    TeamPhoto = null,
                    MembersAsString = "Leander Märkisch, Paul Kathmann, Hauke Reunitz",
                    IsActive = false
                },
                new Team
                {
                    Name = "WeTakeHealthCare",
                    ActiveSince = "Oct 2018",
                    Description = "We support the foreign medical tourists in the choice of the treated hospital and in the implementation of the medical treatment in Germany. With the help of the internet platform WeTakeHealthCare we would like to expand the medical tourism in Germany.",
                    Email = "wetakehealthcare@gmail.com",
                    WebsiteUrl = "https://www.wetakehealthcare.de",
                    FacebookUrl = "WeTakeHealthCare",
                    InstagramUrl = null,
                    LogoImage = team_images[20],
                    TeamPhoto = team_images[19],
                    MembersAsString = "Peter Krieger, Yannick Pietschmann, Marta Golabek",
                    IsActive = false
                },
                new Team
                {
                    Name = "Winter Team VT",
                    ActiveSince = "Oct 2018",
                    Description = "Enhancing the readability of written Text in lectures in diverse situations, using modern light-technology and chemistry",
                    Email = null,
                    WebsiteUrl = null,
                    FacebookUrl = null,
                    InstagramUrl = null,
                    LogoImage = null,
                    TeamPhoto = null,
                    MembersAsString = "Christian Winter",
                    IsActive = false
                },
                new Team
                {
                    Name = "Zircle",
                    ActiveSince = "Oct 2018",
                    Description = "We know that we cannot continue with the wasteful use of resources, we have to apply a new behaviour in our lifestyle - it's called sharing. The sharing economy leader is BlaBlaCar as Uber and Airbnb are no players in the sharing economy. There are some platforms trying to enable sharing goods between people - but most of them are not good at all. As timing is the main factor whether a idea is going to fail or succeed, we think the time has come to make sharing great again! Focusing on students in the beginning and moving on to other target groups, our slightly different approach how sharing could be done in the future will make it more attractive to people to share. An intuitive progressive web application will be the entry point to a market where we see lots of potential to make a meaningful impact in sharing resources. Together. We share.",
                    Email = null,
                    WebsiteUrl = null,
                    FacebookUrl = null,
                    InstagramUrl = null,
                    LogoImage = team_images[21],
                    TeamPhoto = null,
                    MembersAsString = "Jayesh, Arthur, Enxhi, Andy, Frederik, Mario, Sven",
                    IsActive = false
                }
            };
        }

        private static void AddPrizes()
        {
            prizes = new[]
            {
                new Prize
                {
                    Name = "1st place",
                    Reward = "3000 € + 6 months at the PionierGarage Launchpad",
                    RewardValue = 3960,
                    Description = null,
                    Winner = teams.Single(t => t.Name == "TortenGlück"),
                    IsPublic = true
                },
                new Prize
                {
                    Name = "2nd place",
                    Reward = "2000 € + 3 months at the PionierGarage Launchpad",
                    RewardValue = 2480,
                    Description = null,
                    Winner = teams.Single(t => t.Name == "Studentenfutter"),
                    IsPublic = true
                },
                new Prize
                {
                    Name = "3rd place",
                    Reward = "1000 € + 1 month at the PionierGarage Launchpad",
                    RewardValue = 1160,
                    Description = null,
                    Winner = teams.Single(t => t.Name == "SecureRadiationLab"),
                    IsPublic = true
                },
                new Prize
                {
                    Name = "Best Product",
                    Reward = "1000 € granted by Kolibri Games",
                    RewardValue = 1000,
                    Description = null,
                    GivenBy = partners.Single(p => p.Name == "Kolibri Games"),
                    Winner = teams.Single(t => t.Name == "TortenGlück"),
                    IsPublic = true
                },
                new Prize
                {
                    Name = "Most Scalable",
                    Reward = "1000 € + mentoring through LEA Partners",
                    RewardValue = 1200,
                    Description = null,
                    GivenBy = partners.Single(p => p.Name == "LEA Partners"),
                    Winner = teams.Single(t => t.Name == "HelioPas AI"),
                    IsPublic = true
                },
                new Prize
                {
                    Name = "Most Investment-Ready",
                    Reward = "Investment offer by First Momentum Ventures",
                    RewardValue = 0,
                    Description = null,
                    GivenBy = partners.Single(p => p.Name == "First Momentum Ventures"),
                    Winner = teams.Single(t => t.Name == "Read!"),
                    IsPublic = true
                },
            };
        }


        private static TNavigation[] AddReferencesInCollection<TEntity, TNavigation>(TEntity entity, TNavigation[] navigations, Action<TNavigation, TEntity> linkingFunction)
        {
            foreach (var navEntity in navigations)
                linkingFunction(navEntity, entity);
            return navigations;
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
            return 0;

            if (!CurrentIds.ContainsKey(typeof(TType)))
                CurrentIds.Add(typeof(TType), 0);

            CurrentIds[typeof(TType)] = CurrentIds[typeof(TType)] + 1;

            return CurrentIds[typeof(TType)];
        }
        
    }
}
