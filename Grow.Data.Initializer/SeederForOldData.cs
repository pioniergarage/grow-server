using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Grow.Data;
using Grow.Data.Entities;

namespace Grow.Data.Initializer
{
    public static class SeederForOldData
    {
        public static bool IsEnabled { get; set; }
        public static bool EntitiesAdded { get; set; }

        public static Organizer[] Organizers { get; private set; }
        public static Judge[] Judges { get; private set; }
        public static Mentor[] Mentors { get; private set; }
        public static Event[] Events { get; private set; }
        public static Partner[] Partners { get; private set; }
        public static File[] Event_images { get; private set; }
        public static File[] Orga_images { get; private set; }
        public static File[] Judge_images { get; private set; }
        public static File[] Mentor_images { get; private set; }
        public static File[] Team_images { get; private set; }
        public static File[] Partner_images { get; private set; }
        public static Team[] Teams { get; private set; }
        public static Prize[] Prizes { get; private set; }

        static SeederForOldData()
        {
            IsEnabled = true;
            EntitiesAdded = false;

            AddImages();
            AddEvents();
            AddPartners();
            AddPeople();
            AddTeams();
            AddPrizes();
        }

        public static void SeedDataFrom2017(this GrowDbContext context)
        {
            // Add contest
            var contest = new Contest
            {
                Name = "GROW 2017/18",
                Year = "2017",
                IsActive = true,
                Language = "German"
            };
            
            context.Contests.Add(contest);
            context.SaveChanges();
            
            contest.Teams = Teams.Skip(33).SetIds(contest.Id);
            contest.Prizes = Prizes.Skip(6).SetIds(contest.Id);
            contest.Mentors = Mentors.Skip(16).SetIds(contest.Id);
            contest.Judges = Judges.Skip(6).SetIds(contest.Id);
            contest.Organizers = Organizers.Skip(6).SetIds(contest.Id);
            contest.Partners = Partners.Skip(8).SetIds(contest.Id);
            contest.Events = Events.Skip(8).SetIds(contest.Id);

            context.SaveChanges();
        }

        public static void SeedDataFrom2018(this GrowDbContext context)
        {
            // Add contest
            var contest = new Contest
            {
                Name = "GROW 2018/19",
                Year = "2018",
                IsActive = true,
                Language = "English"
            };
            
            context.Contests.Add(contest);
            context.SaveChanges();
            
            contest.Teams = Teams.Take(33).SetIds(contest.Id);
            contest.Prizes = Prizes.Take(6).SetIds(contest.Id);
            contest.Mentors = Mentors.Take(16).SetIds(contest.Id);
            contest.Judges = Judges.Take(6).SetIds(contest.Id);
            contest.Organizers = Organizers.Take(6).SetIds(contest.Id);
            contest.Partners = Partners.Take(8).SetIds(contest.Id);
            contest.Events = Events.Take(8).SetIds(contest.Id);

            context.SaveChanges();
        }
        
        private static void AddPeople()
        {
            // Judges
            Judges = new[] {

                // Judges 2018

                new Judge
                {
                    Name = "Holger Kujath",
                    JobTitle = "Founder and CEO of the online chat community Knuddels",
                    Image = Judge_images[2]
                },
                new Judge
                {
                    Name = "Orestis Terzidis",
                    JobTitle = "Professor and head of the entrepreneurial institute EnTechnon",
                    Image = Judge_images[5]
                },
                new Judge
                {
                    Name = "Michael Kimmig",
                    JobTitle = "Head of Corporate Process Management at GRENKE digital",
                    Image = Judge_images[4]
                },
                new Judge
                {
                    Name = "Bernhard Janke",
                    JobTitle = "Principal at the VC company LEA Partners",
                    Image = Judge_images[0]
                },
                new Judge
                {
                    Name = "Martin Trenkle",
                    JobTitle = "Founder and CEO of the job placement service Campusjäger",
                    Image = Judge_images[3]
                },
                new Judge
                {
                    Name = "Daniel Stammler",
                    JobTitle = "Co-Founder and Co-CEO of the mobile game company Kolibri Games",
                    Image = Judge_images[1]
                },

                // Judges 2017
                
                new Judge
                {
                    Name = "Holger Kujath",
                    JobTitle = "Founder and CEO of the online chat community Knuddels",
                    Image = Judge_images[2]
                },
                new Judge
                {
                    Name = "Orestis Terzidis",
                    JobTitle = "Professor and head of the entrepreneurial institute EnTechnon",
                    Image = Judge_images[5]
                },
                new Judge
                {
                    Name = "Christian Roth",
                    JobTitle = "Managing Partner at the VC company LEA Partners",
                    Image = null
                },
                new Judge
                {
                    Name = "Sven Häwel",
                    JobTitle = "Internet entrepreneur and coach",
                    Image = null
                },
                new Judge
                {
                    Name = "Matthias Hornberger",
                    JobTitle = "Chairman of the CyberForum",
                    Image = null
                },
                new Judge
                {
                    Name = "Nestor Rodriguez",
                    JobTitle = "Managing Director at atrineo AG",
                    Image = Mentor_images[13]
                }
            };

            // Organizers
            Organizers = new[] {

                // Organizers 2018

                new Organizer
                {
                    Name = "Dominik Doerner",
                    Image = Orga_images[3],
                    JobTitle = "Main Coordination"
                },
                new Organizer
                {
                    Name = "Anne-Cathrine Eimer",
                    Image = Orga_images[0],
                    JobTitle = "Workshop Program"
                },
                new Organizer
                {
                    Name = "Christian Wiegand",
                    Image = Orga_images[2],
                    JobTitle = "Fundraising"
                },
                new Organizer
                {
                    Name = "Jasmin Riedel",
                    Image = Orga_images[4],
                    JobTitle = "Event Management"
                },
                new Organizer
                {
                    Name = "Martin Thoma",
                    Image = Orga_images[5],
                    JobTitle = "Mentoring Program"
                },
                new Organizer
                {
                    Name = "Antonia Lorenz",
                    Image = Orga_images[1],
                    JobTitle = "Marketing"
                },

                // Organizers 2017
                
                new Organizer
                {
                    Name = "Dominik Doerner",
                    Image = Orga_images[3],
                    JobTitle = "Marketing"
                },
                new Organizer
                {
                    Name = "Anne-Cathrine Eimer",
                    Image = Orga_images[0],
                    JobTitle = "Fundraising"
                },
                new Organizer
                {
                    Name = "Jasmin Riedel",
                    Image = Orga_images[4],
                    JobTitle = "Event Management"
                },
                new Organizer
                {
                    Name = "Martin Thoma",
                    Image = Orga_images[5],
                    JobTitle = "Main Coordination"
                },
            };

            // Mentors
            Mentors = new[] {

                // Mentors 2018

                new Mentor
                {
                    Name = "Sebastian Böhmer",
                    JobTitle = "Founding Partner at First Momentum Ventures",
                    Expertise = "Venture capital, business development, finance, legal",
                    Description = null,
                    WebsiteUrl = "https://www.linkedin.com/in/sebastianboehmer/",
                    Image = Mentor_images[15]
                },
                new Mentor
                {
                    Name = "Hans-Lothar Busch",
                    JobTitle = "Trainer & coach for the acquisition of industry projects",
                    Expertise = "Acquisition of industry projects, marketing, leadership, purchasing",
                    Description = null,
                    WebsiteUrl = "https://www.mehr-industrieprojekte.de/%C3%BCber-mich/",
                    Image = Mentor_images[4]
                },
                new Mentor
                {
                    Name = "Murat Ercan",
                    JobTitle = "CEO at MEK Webdesign",
                    Expertise = "Online Marketing, Performance Marketing, Social Media, Webdesign and Sales ",
                    Description = null,
                    WebsiteUrl = null,
                    Image = Mentor_images[12]
                },
                new Mentor
                {
                    Name = "Andreas Fischer",
                    JobTitle = "Founding Partner at First Momentum Ventures",
                    Expertise = "Idea validation, strategy, financing ",
                    Description = null,
                    WebsiteUrl = "https://www.linkedin.com/in/afischerfmv/",
                    Image = Mentor_images[0]
                },
                new Mentor
                {
                    Name = "Jonas Fuchs",
                    JobTitle = "Founder & CEO at Usertimes Solution GmbH",
                    Expertise = "Student perspective, MVPs, networking, company culture, sales ",
                    Description = null,
                    WebsiteUrl = null,
                    Image = Mentor_images[7]
                },
                new Mentor
                {
                    Name = "Peter Greiner",
                    JobTitle = "Active and financial investor for startups",
                    Expertise = "Business modelling, founding, organizational design, financing, sales ",
                    Description = null,
                    WebsiteUrl = "https://www.xing.com/profile/Peter_Greiner25",
                    Image = Mentor_images[14]
                },
                new Mentor
                {
                    Name = "Cécile F. Heger",
                    JobTitle = "CEO at ABC-Vidal",
                    Expertise = "Finance, salaries, organization, strategy ",
                    Description = null,
                    WebsiteUrl = "https://www.linkedin.com/in/c%C3%A9cile-f-heger-1593a062",
                    Image = Mentor_images[2]
                },
                new Mentor
                {
                    Name = "Manuel Köcher",
                    JobTitle = "Manager at the CIE (KIT) and business consultant",
                    Expertise = "Organizational design, market analysis, product development, strategy ",
                    Description = null,
                    WebsiteUrl = null,
                    Image = Mentor_images[10]
                },
                new Mentor
                {
                    Name = "Karl Lorey",
                    JobTitle = "Founding Partner at First Momentum Ventures",
                    Expertise = "Scraping, machine learning, MVP, web development, financing ",
                    Description = null,
                    WebsiteUrl = "https://www.linkedin.com/in/karllorey/",
                    Image = Mentor_images[8]
                },
                new Mentor
                {
                    Name = "Maja Malovic",
                    JobTitle = "Business Innovation Manager and Design Thinking Coach",
                    Expertise = "Business modelling, design thinking, client communication, prototyping",
                    Description = null,
                    WebsiteUrl = "https://www.linkedin.com/in/maja-malovic-a5095b163/",
                    Image = Mentor_images[9]
                },
                new Mentor
                {
                    Name = "Jannik Nefferdorf",
                    JobTitle = "Student and entrepeneurial enthusiast",
                    Expertise = "Startup ecosystem, business modelling, entrepeneurship basics ",
                    Description = null,
                    WebsiteUrl = "https://www.linkedin.com/in/neffi97/",
                    Image = Mentor_images[6]
                },
                new Mentor
                {
                    Name = "Martin Rammensee",
                    JobTitle = "Founder & CEO at Green Parrot GmbH",
                    Expertise = "Strategy, market entry, mobility, B2B ",
                    Description = null,
                    WebsiteUrl = "https://www.linkedin.com/in/martin-rammensee-a7860398/",
                    Image = Mentor_images[11]
                },
                new Mentor
                {
                    Name = "Nestor Rodriguez",
                    JobTitle = "Innovational tourist",
                    Expertise = "Market, startup scene, business development, inspiration ",
                    Description = "Managing Director at atrineo AG",
                    WebsiteUrl = "https://www.xing.com/profile/Nestor_Rodriguez",
                    Image = Mentor_images[13]
                },
                new Mentor
                {
                    Name = "Ben Romberg",
                    JobTitle = "Founder of codefortynine GmbH",
                    Expertise = "Bootstrapping, lean startup, continuous delivery, business modelling, Y Combinator \"philosophy\"",
                    Description = null,
                    WebsiteUrl = "https://www.xing.com/profile/Ben_Romberg",
                    Image = Mentor_images[1]
                },
                new Mentor
                {
                    Name = "Heinz T. Rothermel",
                    JobTitle = "Business consultant and lecturer",
                    Expertise = "Business planning, strategy, venture capital, marketing, communication ",
                    Description = null,
                    WebsiteUrl = "https://www.xing.com/profile/Heinz_Rothermel/",
                    Image = Mentor_images[5]
                },
                new Mentor
                {
                    Name = "Frederic Tausch",
                    JobTitle = "CTO & Co-Founder at apic.ai",
                    Expertise = "AI, software development, bootstrapping, founding, student perspective ",
                    Description = null,
                    WebsiteUrl = "https://www.linkedin.com/in/frederic-tausch/",
                    Image = Mentor_images[3]
                },
                
                // Mentors 2017
                
                new Mentor
                {
                    Name = "Peter Greiner",
                    JobTitle = "Active and financial investor for startups",
                    Expertise = "Business modelling, founding, organizational design, financing, sales ",
                    Description = null,
                    WebsiteUrl = "https://www.xing.com/profile/Peter_Greiner25",
                    Image = Mentor_images[14]
                },
                new Mentor
                {
                    Name = "Heinz T. Rothermel",
                    JobTitle = "Business consultant and lecturer",
                    Expertise = "Business planning, strategy, venture capital, marketing, communication ",
                    Description = null,
                    WebsiteUrl = "https://www.xing.com/profile/Heinz_Rothermel/",
                    Image = Mentor_images[5]
                },
                new Mentor
                {
                Name = "Alexander Glöckner",
                JobTitle = "Berater bei Glöcner & Schuhwerk",
                Expertise = "Strategie, Business Process Management, Unternehmensorgainsation, Risikomanagement, Qualität, Datenschutz und IT-Sicherheit",
                Description = "Seit ca. 20 Jahren befasse und berate ich im Qualitätsmanagement Unternehmer und Führungskräfte ihre Geschäftsprozesse zu verbessern bzw ihr Unternehmen zielführend zu organisieren. Ganz nach meinem Firmenmotto, Qualität kostet weniger als keine Qualität, untersütze ich auch seit ca. einem Jahr Startups Fehler in der Gründungsphase zu vermeiden.",
                WebsiteUrl = null,
                Image = null
                },
                new Mentor
                {
                Name = "Anita Berres",
                JobTitle = "Selbstständige Unternehmerin",
                Expertise = "Vertrieb, Team-Entwicklung, Kreativität, Konfliktmanagement.",
                Description = "14 Jahre erfolgreiche Vertriebstätigkeit für Arbeitgeber in der IT-Branche (Direktvertrieb, Fachhandels-Vertrieb, Aufbau und Leitung Key Account Management) /// 21 Jahre selbstständig mit den Themen Vertrieb, Strategie, Team",
                WebsiteUrl = null,
                Image = null
                },
                new Mentor
                {
                Name = "Bianka Reinhardt",
                JobTitle = "Selbständige Marketingberaterin und Coach, Referentin für Marketing und Öffentlichkeitsarbeit bei der CardProcess GmbH",
                Expertise = "Persönlichkeitsentwicklung, Zeitmanagement, Präsentations- und Kommunikationstechniken, Entwicklung von Marketingstrategien und Kommunikationskonzepten für die interne Kommunikation und Öffentlichkeitsarbeit, Markenpositionierung und Aufbau der Corporate Identity, Aufbau und Strukturierung von Marketingabteilungen, Organisation von Marketingprozessen, Budgeterstellung und -management.",
                Description = "Bianka Reinhardt ist seit mehr als 25 Jahren im Bereich Marketing und Kommunikation tätig, arbeitet als selbständige Beratierin und Business Coach. Sie hat verschiedene Marketingpositionen mit Führungsverantwortung in mittelständischen Unternehmen in den Branchen IT, Internet, eCommerce und in der Finanzdienstleistungsbranche bekleidet. Sie weiß aus ihrer Berufserfahrung um die Herausforderungen von StartUps und Unternehmen, die im Aufbau begriffen sind. Sie stammt aus einem Unternehmerhaushalt und kennt die Anforderungen an eine selbstständige Tätigkeit.",
                WebsiteUrl = null,
                Image = null
                },
                new Mentor
                {
                Name = "Claudia März-Sax",
                JobTitle = "Selbstständige Beraterin für die Unternehmen s.a.x. Karlsruhe und GDEKK Köln",
                Expertise = "Präsentationstraining, erstellen von Businessplänen u.v.",
                Description = "Umfassende Berufserfahrungen - siehe Profil auf Xing oder LinkedIn 1998 Gründerin Medical Columbus AG Königstein - erfolgreicher Startup gegründet als AG! Erfahrung von Investorensuche bis Börsengang.",
                WebsiteUrl = null,
                Image = null
                },
                new Mentor
                {
                Name = "Dirk Keune",
                JobTitle = "Geschäfsführer bei der Inventioncase Beteiligungs GmbH",
                Expertise = "Business Pläne, Präsentationen, Vertriebkonzepte und Strategien, Business Development, Innovationsmanagement",
                Description = "20 Jahre Selbstständig, 5 GmbHs und eine Aktiengesellschaft gegründet (alle noch existent), 17 Patente angemeldet. Ich liebe gute Ideen, auch wenn sie auf den ersten Blick keinen Erfolg versprechen. Aus Ideen Potential zu schöpfen und neue Themen ausarbeiten und an den Markt bringen - das wäre mein Hobby, wenn es nicht mein Beruf wäre. Dieses Wissen setze ich selbst ein, teile es aber auch gerne.",
                WebsiteUrl = null,
                Image = null
                },
                new Mentor
                {
                Name = "Dr. Klaus Neb",
                JobTitle = "AR Vorsitzender Michelin DE",
                Expertise = "Business Model Kundenorientierung",
                Description = "Produktion Verwaltung Vertrieb In internationalem Umfeld Begleitung von Consultant- und IT- Unternehmen als AR",
                WebsiteUrl = null,
                Image = null
                },
                new Mentor
                {
                Name = "Florian Buzin",
                JobTitle = "CEO bei STARFACE GmbH",
                Expertise = "Vom Startup zum Wachstumsunternehmen - Technik, Strategie, Reporting, Go to market ...",
                Description = "Mein erstes Unternehmen gründete ich mit 18. Nicht jedes wurde ein großer Erfolg, doch lernen könnte man dabei viel. Mit STARFACE konnten wir erfolgreich VC Geld akquirieren und das anorganische Wachstum kennenlernen. Von 5 auf >80 Mitarbeiter in einem technologisch sehr fördernden Markt. Parallel engagiere ich mich seit einiger Zeit als Angel Investor bei spannenden Projekten.",
                WebsiteUrl = null,
                Image = null
                },
                new Mentor
                {
                Name = "Francesco Loth",
                JobTitle = "Geschäftsführer bei ETECTURE",
                Expertise = "finance, validierung business model, netzwerk",
                Description = "Francesco Loth ist seit über 15 Jahren als Unternehmer und Geschäftsführer in der digitalen Welt tätig. Er war maßgeblich an der Neugründung und den Ausbau von 2 Agenturen beteiligt. Zuletzt war er Geschäftsführer bei der UDG nach einem erfolgreichen Asset Deal. Aktuell ist Francesco Loth Geschäftsführer der ETECTURE und Gründer eines Maschinenbauunternehmens Karlsruhe Schwerpunkt Sondermaschinenbau und Automatisierung.",
                WebsiteUrl = null,
                Image = null
                },
                new Mentor
                {
                Name = "Gunnar Lott",
                JobTitle = "Geschäftsführer Visibility Communications",
                Expertise = "Marketing, Storytelling, Pitching, Marktfähigkeitsprüfung",
                Description = "Langjähriger Medienmanager, jetzt Agenturinhaber und nebenberuflicher Influencer. Games-Experte. Journalist.",
                WebsiteUrl = null,
                Image = null
                },
                new Mentor
                {
                Name = "Hans-Georg Edlefsen",
                JobTitle = "Freier Berater",
                Expertise = "Witschaftlichkeitsanalysen",
                Description = "35 Jahre Berufserfahrung in unterschiedlichen Branchen (von Chemie über Handel, Konsumgüter bis zur Energiewi.) mit Erfahrungsschwerpunkten im Controlling, in der allgemeinen Unternehmensführung (als kfm GF und kfm Vorstand) und in der Energiewirtschaft",
                WebsiteUrl = null,
                Image = null
                },
                new Mentor
                {
                Name = "Heiko Stapf",
                JobTitle = "Geschäftsführer Cyber Manufaktur und Emendare",
                Expertise = "Agiles Produkt Management / Exploration, Ideen validieren, Design Thinking, Strategien, Entwicklungsprozesse",
                Description = "\"Keep calm and innovate\" Heiko Stapf ist Certified Scrum Trainer. Sein besonderes Interesse gilt der Entwicklung innovativer Produkte und Dienstleistungen und der Rolle des Product Owners in Scrum. Er ist Geschäftsführer der UX Agentur Cyber Manufaktur GmbH und der agilen Unternehmensberatung Emendare GmbH & Co. KG. Agile Methoden sind für Ihn der Schlüssel für eine erfolgreiche Produktentwicklung.",
                WebsiteUrl = null,
                Image = null
                },
                new Mentor
                {
                Name = "Jan Hichert",
                JobTitle = "Business Angel",
                Expertise = "Teams finden, Prototypen bauen, Ideen validieren, Geschäftsmodelle entwickeln, Strukturieren und Finanzieren",
                Description = "Jan Hichert ist seit 17 Jahren als Entrepreneur und Business Angel in Deutschland und USA aktiv. Er hat selbst zwei Unternehmen aufgebaut und mehrere Startups finanziert. Jan liebt die Zusammenarbeit mit enthusiastischen Gründern und das kreative Chaos in Startups.",
                WebsiteUrl = null,
                Image = null
                },
                new Mentor
                {
                Name = "Jan Schöttelndreier",
                JobTitle = "VP eCommerce Solutions bei asknet AG / freier Berater",
                Expertise = "Ideen validieren, Business Cases, Vertrieb & Prozesse bauen",
                Description = "Generalist, 15 Jahre im Produktmanagement, Vertrieb, Marketing und Geschäftsführung. Zunächst Financial Services, dann eCommerce, Software, Internet.",
                WebsiteUrl = null,
                Image = null
                },
                new Mentor
                {
                Name = "Joachim Tatje",
                JobTitle = "Gründer und Inhaber der PR Agentur \"ViATiCO Strategie und Text\"",
                Expertise = "Kommunikation planen, Namen und Slogans entwickeln, praktische Öffentlichkeitsarbeit, Messeauftritte planen",
                Description = "Joachim Tajte hat Elektrotechnik an der Uni KA studiert und danach etliche Jahre Elektronik entwickelt. Der Wechsel zu einem Start-up Unternehmen als Marketing- und Vertriebsleiter entsprach seinemTalent zu kreativen Tätigkeiten. Nach knapp acht Jahren erfolgreicher internationaler Tätigkeit machte sich Tatje 1993 als PR-Berater selbstständig. Seither berät und begleitet er mittelständische Unternehmen in allen Belangen der Unternehmenskommunikation.",
                WebsiteUrl = null,
                Image = null
                },
                new Mentor
                {
                Name = "Johannes Häfele",
                JobTitle = "Business Angel",
                Expertise = "Pricing-Modelle, E-Commerce, Finanzplanung",
                Description = "Johannes Häfele war viele Jahre als CFO im B2B und B2C-Handel tätig. Seit 56 Jahren ist er im Cyberforum Karlsruhe aktiver Business Angel und Mentor für Startups (u.a. \"Meine Spielzeugkiste\")",
                WebsiteUrl = null,
                Image = null
                },
                new Mentor
                {
                Name = "Klaus Welle",
                JobTitle = "CTO und Mitgründer von Selfbits",
                Expertise = "Ideen validieren, Konzeption und Praxistipps bei Anwendungsentwicklung (mobil, desktop, web)",
                Description = "Klaus ist seit 2015 als Mitgründer der Selfbits GmbH in der Karlsruher Startupszene aktiv. Als CTO ist er für die Selfbits Cloud Plattform sowie für die Entwicklung von Web- und Mobile-Anwendungen für Kunden verantwortlich. Die Erkenntnisse aus der Zeit als Existenzgründer sowie die Erfahrung in der schnellen Realisierung von Business-Anwendungen gibt er im Rahmen von Grow gerne weiter.",
                WebsiteUrl = null,
                Image = null
                },
                new Mentor
                {
                Name = "Marc Zacherl",
                JobTitle = "Co-founder, Geschäftsführer",
                Expertise = "Business Development, Marketing, Sales, IT, Pitch",
                Description = "Ich bin überzeugt, dass meine bisherigen Erfahrungen aus verschiedenen Projekten in der Unternehmensberatung und auch in meinem eigenen Start-up erfolgreich beitragen könnten. Ich möchte Menschen helfen und sie dabei unterstützen Ihre Ideen zu verwirklichen und Ihre Träume leben zu können. Gerade im IT Bereich, Business Development, Aufbau von Teams und Bereichen, Vertrieb, Marketing und Netzwerke kann ich sehr gut beitragen. Der Weg auf die Spitze ist eisern, jedoch lässt er sich mit Motivation, Engagement, Wissen und Durchhaltervermögen erreichen â€“ dies möchte ich ebenso übermitteln und weitergeben.",
                WebsiteUrl = null,
                Image = null
                },
                new Mentor
                {
                Name = "Mathias Thomas",
                JobTitle = "Inhaber Dr. Thomas + Partner",
                Expertise = "Prozesse / logistik / Projektmanagement / BMD / Pitch Training",
                Description = "Mathias is managing director and owner of Dr. Thomas + Partner GmbH & Co. KG. Dr. Thomas + Partner is one of the top logistics and software development companies in Europe providing the logistical IT for brands such as Pfizer, Otto or Hyundai.Mathias has great expertise in logistics, eCommerce and future technologies.He is a frequent keynote speaker promoting the idea of a truly local and sustainable approach of online business development.",
                WebsiteUrl = null,
                Image = null
                },
                new Mentor
                {
                Name = "Matthias Schultze",
                JobTitle = "Geschäftsführer bei TechniData IT-Service GmbH",
                Expertise = "Marketing, Entscheiderpräsentationen vorbereiten, Business Plan, Kontakte/Netzwerk, Technologische Unterstützung (bspw. Hosting einer Plattform in unserem eigenen RZ)",
                Description = "Der rote Faden in meiner mittlerweile über 30jährigen beruflichen Tätigkeit findet sich in den Themen \"IT\" sowie \"Führung und Management\".Aufbauend auf meinem Studium der Wirtschaftsinformatik(BA) und nach(leitenden) Funktionen in den Bereichen IT, Vertrieb, Architekturmanagement, Marketing und Einkauf in den Branchen \"Energieversorgung\" und \"Finanzdienstleistung\", bin ich seit September 2014 bei der TechniData IT-Service GmbH und dort seit April 2015 als Geschäftsführer tätig.Mit dem Kontakt zu Innovationstreibern und Start-Ups beschäftige ich mich schon seit gut 20 Jahren, bekomme hieraus viele Impulse und gebe im Gegenzug gerne meine Erfahrungen weiter.",
                WebsiteUrl = null,
                Image = null
                },
                new Mentor
                {
                Name = "Matthias Schürer",
                JobTitle = "Präsident BWB",
                Expertise = "Business Model verkaufen",
                Description = "GF verschiedener Unternehmen. Mentor bei Start Ups, Karlsruhe und Absolventum, Mannheim.",
                WebsiteUrl = null,
                Image = null
                },
                new Mentor
                {
                Name = "Michael Rausch",
                JobTitle = "COO",
                Expertise = "Vertrieb & Akquise, Geschäftsführung",
                Description = "Vertrieb seit 1990, E-Commerce seit 1998, Führungserfahrung seit 1997",
                WebsiteUrl = null,
                Image = null
                },
                new Mentor
                {
                Name = "Oliver Kuppler",
                JobTitle = "CEO/Geschäftsführer bei Selfbits GmbH",
                Expertise = "Business Model, Sales, Web und Mobile App-Development, Backend/Cloud",
                Description = "IT und FS Consulting @ KPMG, Co-Founder @ Selfbits",
                WebsiteUrl = null,
                Image = null
                },
                new Mentor
                {
                Name = "Sophie Horstmann",
                JobTitle = "Geschäftsführung der LAFAM Holding GmbH",
                Expertise = "Ideen validieren, Geschäftsmodelle prüfen und diskutieren, Bottom Up Marktanalyse, B2B Vertriebsstrategie entwickeln, Durchsprache Pitch Deck mit den Bestandteilen Markt, Wettbewerb, Geschäftsmodell, Kundennutzen, Finanzen, Produkt/Service, Marketing/Vertrieb, USPs",
                Description = "Sophie Horstmann studierte Wirtschaftsingenieurwesen mit dem Schwerpunkt Maschinenbau. Nach dem Studium arbeitete sie für ein international tätiges Maschinen- und Anlagenbauunternehmen im technischen Vertrieb(Schwerpunkt China). Seit dem Frühjahr 2015 ist Sophie Horstmann als Investmentmanagerin für die LAFAM Holding GmbH tätig und wurde mit Beginn des Jahres 2017 in die Geschäftsführung berufen.Sophie Horstmann beurteilt in ihrer täglichen Arbeit Businesspläne unterschiedlicher Branchen sowie Produkte, trifft Investitionsentscheidungen und berät die Beteiligungen des Portfolios der LAFAM Holding GmbH strategisch.",
                WebsiteUrl = null,
                Image = null
                },
                new Mentor
                {
                Name = "Stefanie Molzberger",
                JobTitle = "SaaS Sales Leader, IBM Digital Sales DACH",
                Expertise = "Validierung und Beratung in Sachen Channel Model, B2B Vertrieb, Business Model",
                Description = "Ich bin seit 20 Jahren in der IT Branche und habe in dieser Zeit die Themen Channel / Business Development / Marketing und Sales uns unterschiedlichsten Blickwinkeln kennen gelernt und mir in dieser Zeit eine breite Expertise in diesen Feldern aufgebaut.Desweiteren durfte/konnte ich 2,5 Jahre mit Startups zusammenarbeiten und hierbei viele praktische Ansätze & Mentorings erfahren.",
                WebsiteUrl = null,
                Image = null
                },
                new Mentor
                {
                Name = "Tanja Müller",
                JobTitle = "Leitung Mentoring & Coaching",
                Expertise = "Pitchtraining, Business Model Development, Kontakte, Venture Captial Finanzierungen",
                Description = "Tanja Müller sammelte als gelernte Anwendungsinformatikerin und Industriekauffrau jahrelang Erfahrung im Geschäftsprozessmanagement, die sie heute in die Betreuung von Existenzgründern einbringt.Die analytische Beurteilung von Geschäftsmodellen ist ein Schwerpunkt ihrer Tätigkeit.Dabei bilden ihre persönlichen Erfahrungen in verschiedenen Branchen die Basis, um die Praxistauglichkeit von Business-Ideen zu evaluieren.An ihrem Beruf schätzt sie besonders den direkten Kontakt zu erfahrenen Business Mentoren, wie zu hochmotivierten Gründern, deren spannende Ideen sie immer wieder faszinieren.",
                WebsiteUrl = null,
                Image = null
                },
                new Mentor
                {
                Name = "Tim Riedel",
                JobTitle = "GF bei der eyeworkers interactive GmbH",
                Expertise = "Prozesse und Ideen validieren, Bewertung von Technologien im Webumfeld, Tipps zur Präsentation, Kundenakquise",
                Description = "Ich bin seit 17 Jahren einer der Geschäftsführer bei eyeworkers und kümmere mich mit meinem Team um Kundenakquise und die Entwicklung und Einführung von webbasierter Software im Unternehmensumfeld.",
                WebsiteUrl = null,
                Image = null
                },
                new Mentor
                {
                Name = "Abilio Avila",
                JobTitle = "Doktorand beim EnTechnon, KIT",
                Expertise = "New Product Development, Product Management, (Agile) Project Management, Partner Networks, Business Modeling",
                Description = "Abilio Avila is responsible for the KIT Startup Accelerator upCAT and the coordination of the research activities at the Chair of Entrepreneurship and Technology Management. Furthermore, he is a Research Associate and Ph.D. candidate at KIT (Institute EnTechnon). His research focuses on partner ecosystems in the enterprise software industry.",
                WebsiteUrl = null,
                Image = null
                },
                new Mentor
                {
                Name = "Benedict Heblich",
                JobTitle = "Doktorand beim EnTechnon, KIT",
                Expertise = "Wertebewusstes Entrepreneurship",
                Description = "Benedict Heblich ist Doktorand am EnTechnon (KIT) zum Thema \"Wertebewusstes Entrepreneurship\" sowie selbstständiger Trainer für Persönlichkeitsentwicklung. Er unterstützt in seiner Arbeit insbesondere Gründer und Gründungsinteressierte durch Workshops und individuelle Coachings dabei, die Gestaltung des eigenen Unternehmens auch an der eigenen Persönlichkeit auszurichten. Er arbeitet mit wissenschaftlichen und in der Praxis erprobten Methoden aus der empirischen Psychologie. Dabei liegt ein besonderer Fokus darauf die Gründer und Gründungsinteressierten dabei zu unterstützen, mehr Klarheit über die persönlichen Werte zu erlangen und ihr unternehmerisches Handeln daran zu orientieren. Sowohl Wissenschaft als auch Praxis deuten darauf hin, dass Wertekongruenz und die damit verbundene intrinsische Motivation ein wichtiger Erfolgsfaktor für Unternehmer ist. Weitere Informationen zu Benedict Heblich und seiner Arbeit sowie kostenlosen Zugang zu einem der umfassendsten Persönlichkeitstest zu persönlichen Werten findet ihr unter www.findyourvalues.com.",
                WebsiteUrl = null,
                Image = null
                },
                new Mentor
                {
                Name = "Markus Lau",
                JobTitle = "Doktorand beim EnTechnon, KIT",
                Expertise = "Geschäftsmodellinnovationen, Business Model Management, Business Model Design",
                Description = "Markus Lau ist Doktorand und wissenschaftlicher Mitarbeiter EnTechnon (KIT). Seine Forschung befasst sich mit Herausforderungen und Chancen für Geschäftsmodellinnovationen in Wertschöpfungsnetzwerken und in sich transformierenden Märkten. Er konzentriert sich auf die Entwicklung von unterstützenden Methoden und Werkzeugen für Business Model Management und Business Model Design. Ein besonderer Branchenschwerpunkt liegt auf dem Energiesektor in Deutschland. Markus Lau trainiert Führungskräfte, Doktoranden und Studierende in verschiedenen Bereichen wie Technologie-Entrepreneurship, Business Model Generation und Value Driven Innovation. Er unterrichtet seit 2016 im Executive Programm der Hector Business School (Executive Master und MBA Fundamentals).",
                WebsiteUrl = null,
                Image = null
                },
            };
        }

        private static void AddPartners()
        {
            Partners = new[] {

                // Partners 2018

                new Partner
                {
                    Name = "Knuddels",
                    Description = "Online chat community",
                    Image = Partner_images[9]
                },
                new Partner
                {
                    Name = "LEA Partners",
                    Description = "Local VC company",
                    Image = Partner_images[7]
                },
                new Partner
                {
                    Name = "GRENKE Digital",
                    Description = "Financial service provider",
                    Image = Partner_images[2]
                },
                new Partner
                {
                    Name = "Karlshochschule",
                    Description = "Private international university",
                    Image = Partner_images[4]
                },
                new Partner
                {
                    Name = "First Momentum Ventures",
                    Description = "VC company founded by students",
                    Image = Partner_images[1]
                },
                new Partner
                {
                    Name = "KIT Gründerschmiede",
                    Description = "Project of the KIT supporting R2B",
                    Image = Partner_images[5]
                },
                new Partner
                {
                    Name = "EnTechnon",
                    Description = "Entrepreneurial institute at the KIT",
                    Image = Partner_images[0]
                },
                new Partner
                {
                    Name = "Kolibri Games",
                    Description = "Mobile game developer",
                    Image = Partner_images[6]
                },

                // Partners 2017
                
                new Partner
                {
                    Name = "First Momentum Ventures",
                    Description = "VC company founded by students",
                    Image = Partner_images[1]
                },
                new Partner
                {
                    Name = "KIT Gründerschmiede",
                    Description = "Project of the KIT supporting R2B",
                    Image = Partner_images[5]
                },
                new Partner
                {
                    Name = "Knuddels",
                    Description = "Online chat community",
                    Image = Partner_images[9]
                },
                new Partner
                {
                    Name = "LEA Partners",
                    Description = "Local VC company",
                    Image = Partner_images[7]
                },
                new Partner
                {
                    Name = "CyberForum",
                    Description = "Hightech business network",
                    Image = Partner_images[8]
                },
                new Partner
                {
                    Name = "Sparkasse Karlsruhe",
                    Description = "Local bank",
                    Image = Partner_images[10]
                }
            };
        }

        private static void AddEvents()
        {

            // Events 2018

            Events = new[] {
                new Event
                {
                    Name = "Kickoff",
                    Address = "Karlstraße 36 - 38, 76133 Karlsruhe",
                    Location = "Karlshochschule",
                    Description = "The kickoff is where the fun starts, no matter whether you have already applied or whether you're still undecided. We will present everything you need to know about the contest and give you a chance to find an idea and/or teammates. And the 11 weeks of work will start right away.",
                    ExternalEventUrl = "https://www.facebook.com/events/328499797707535/",
                    Start = new DateTime(2018, 11, 12, 18, 0, 0).ToUniversalTime(),
                    End = new DateTime(2018, 11, 12, 21, 0, 0).ToUniversalTime(),
                    HasTimesSet = true,
                    IsMandatory = true,
                    HeldBy = null,
                    Image = Event_images[0],
                    Visibility = Event.EventVisibility.Public,
                    Type = Event.EventType.MainEvent
                },
                new Event
                {
                    Name = "Seed Day",
                    Address = "Rintheimer Str. 15, 76131 Karlsruhe",
                    Location = "PionierGarage Launchpad",
                    Description = "At the seed day we want to connect teams and mentors. First, you get a short chance to pitch both your idea and what you need help with in front of everyone. Then you can find the most suitable mentors in short one-on-one talks. ",
                    ExternalEventUrl = null,
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
                    ExternalEventUrl = null,
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
                    ExternalEventUrl = null,
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
                    ExternalEventUrl = null,
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
                    ExternalEventUrl = "https://www.facebook.com/events/592799921164435/",
                    Start = new DateTime(2018, 12, 09, 19, 0, 0).ToUniversalTime(),
                    End = new DateTime(2018, 12, 09, 22, 0, 0).ToUniversalTime(),
                    HasTimesSet = true,
                    IsMandatory = true,
                    HeldBy = null,
                    Image = Event_images[1],
                    Visibility = Event.EventVisibility.Public,
                    Type = Event.EventType.MainEvent
                },
                new Event
                {
                    Name = "Workshop \"Financing\"",
                    Address = "Rintheimer Str. 15, 76131 Karlsruhe",
                    Location = "PionierGarage Launchpad",
                    Description = "Financing is about getting the funds necessary to build your idea and/or project, especially using investment money. And who would be better suited to tutor you than the two Karlsruhe-based Venture Capital companies LEA Partners and First Momentum Ventures.",
                    ExternalEventUrl = null,
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
                    ExternalEventUrl = "https://www.facebook.com/events/288450928456010/",
                    Start = new DateTime(2019, 01, 30, 19, 0, 0).ToUniversalTime(),
                    End = new DateTime(2019, 01, 30, 22, 0, 0).ToUniversalTime(),
                    HasTimesSet = true,
                    IsMandatory = true,
                    HeldBy = null,
                    Image = Event_images[2],
                    Visibility = Event.EventVisibility.Public,
                    Type = Event.EventType.MainEvent
                },

                // Events 2017

                new Event
                {
                    Name = "Kickoff",
                    Address = "Haid-und-Neu-Straße 18, 76131 Karlsruhe",
                    Location = "Hoepfner Burg, CyberForum",
                    Description = "Dies ist der Startschuss für einen Gründungswettbewerb, der über die nächsten 11 Wochen alles von den Teams abverlangen wird. Los geht es mit dem Grow Kickoff am 09. November um 19 Uhr. Alle Teams, Mentoren, Partner und Besucher sind herzlich zu diesem Anlass in die Hoepfner Burg, genauer dem Cyberforum eingeladen. Der Abend ist vor allem als ein Teamup zu betrachten. Jedes Team darf kurz seine Idee vorstellen. Studenten, die noch keine Idee haben können anschließend schauen, ob Sie sich einem Team anschließen wollen. Des Weiteren haben die Teams und Mentoren Zeit sich zu finden. Bei Getränken und Snacks wollen wir dann den Abend entspannt ausklinken lassen.",
                    ExternalEventUrl = "https://www.facebook.com/events/198332300709658/",
                    Start = new DateTime(2017, 11, 09, 19, 0, 0).ToUniversalTime(),
                    End = new DateTime(2017, 11, 09, 22, 0, 0).ToUniversalTime(),
                    HasTimesSet = true,
                    IsMandatory = true,
                    HeldBy = null,
                    Image = Event_images[0],
                    Visibility = Event.EventVisibility.Public,
                    Type = Event.EventType.MainEvent
                },
                new Event
                {
                    Name = "Workshop Idee",
                    Address = "Rintheimer Str. 15, 76131 Karlsruhe",
                    Location = "PionierGarage Launchpad",
                    Description = @"Tag 1: Value Driven Innovation
Bei diesem Workshop geht es darum, den Teilnehmer Wissen sowie Werkzeuge an die Hand zu geben, um Geschäftsideen umzusetzen und den Wert für den Kunden zu formulieren, auszubauen und einzuschätzen. Für diesen Zweck werden die Teilnehmer sowohl im Frontalunterricht als auch mit Hands-On-Abschnitten (action learning) mit dem Thema auseinandergesetzt. Im Kern lernen die Teams dabei einen kundenorientierten Prozess, mit welchem ein tieferes Kundenverstehen möglich ist und der Problemraum iterativ verfeinert und definiert werden kann.

Darüber hinaus sollen die Teilnehmer werden die Teilnehmer Werkzeuge erhalten, um ein erstes Business Model für das eigene Projekt zu erkennen, auszubauen und einzuschäten.

Tag 2: Team Core Values
Die Teilnehmer wenden Methoden aus der empirischen Psychologie an, um mehr Klarheit über ihre persönlichen Werte zu erlangen. Basis bildet hierbei ein umfassender wissenschaftlicher Persönlichkeitstest zu persönlichen Werten. Darauf aufbauend einigen sich die Teams auf gemeinsame Kernwerte, die als Leitprinzipien für das Unternehmen dienen können. Aus diesen Kernwerten wird ein Unternehmenszweck abgeleitet.

Im zweiten Teil des Workshops am 04. Dez. wird der Unternehmensweck über die strukturierte Einbettung in die einzelnen Facetten des Geschäftsmodells operationalisiert. Hierdurch wird die Wertekongruenz im Unternehmen gefördert. Sowohl Wissenschaft als auch Praxis deuten darauf hin, dass Wertekongruenz und die damit verbundene intrinsische Motivation ein wichtiger Erfolgsfaktor für Unternehmer ist. ",
                    ExternalEventUrl = "https://www.facebook.com/events/198332300709658/",
                    Start = new DateTime(2017, 11, 13, 9, 30, 0).ToUniversalTime(),
                    End = new DateTime(2017, 11, 14, 16, 30, 0).ToUniversalTime(),
                    HasTimesSet = true,
                    IsMandatory = true,
                    HeldBy = null,
                    Visibility = Event.EventVisibility.ForAllTeams,
                    Type = Event.EventType.Workshop
                },
                new Event
                {
                    Name = "Donnerstagstreffen 1",
                    Address = "Rintheimer Str. 15, 76131 Karlsruhe",
                    Location = "PionierGarage Launchpad",
                    Description = "Mit diesen Treffen möchten wir gezielt den Austausch mit den Mentoren fördern. In einer geselligen Runde könnte ihr mit den Mentoren Probleme diskutieren, Fragen stellen und Feedback einholen.",
                    ExternalEventUrl = null,
                    Start = new DateTime(2017, 11, 16, 19, 30, 0).ToUniversalTime(),
                    End = new DateTime(2017, 11, 16, 21, 0, 0).ToUniversalTime(),
                    HasTimesSet = true,
                    IsMandatory = false,
                    HeldBy = null,
                    Visibility = Event.EventVisibility.ForAllTeams,
                    Type = Event.EventType.Mentoring
                },
                new Event
                {
                    Name = "Donnerstagstreffen 2",
                    Address = "Rintheimer Str. 15, 76131 Karlsruhe",
                    Location = "PionierGarage Launchpad",
                    Description = "Mit diesen Treffen möchten wir gezielt den Austausch mit den Mentoren fördern. In einer geselligen Runde könnte ihr mit den Mentoren Probleme diskutieren, Fragen stellen und Feedback einholen.",
                    ExternalEventUrl = null,
                    Start = new DateTime(2017, 11, 30, 19, 30, 0).ToUniversalTime(),
                    End = new DateTime(2017, 11, 30, 21, 0, 0).ToUniversalTime(),
                    HasTimesSet = true,
                    IsMandatory = false,
                    HeldBy = null,
                    Visibility = Event.EventVisibility.ForAllTeams,
                    Type = Event.EventType.Mentoring
                },
                new Event
                {
                    Name = "Workshop Geschäftsmodell",
                    Address = "Rintheimer Str. 15, 76131 Karlsruhe",
                    Location = "PionierGarage Launchpad",
                    Description = @"Tag 1: Value Driven Innovation
Fortsetzung vom Workshop vom 13. Nov.

Tag 2: Fast Forward Business Modeling
Das Design von Business Models hat erheblichen Einfluss auf die Entwicklung und den Erfolg einer Unternehmung. In diesem Workshop nutzen die Teilnehmer verschiedene Methoden zur vereinfachten und aggregierten Abbildung der relevanten Aktivitäten einer Unternehmung. Der Abstraktionsgrad der Methoden ermöglicht zum einen eine schnelle Konkretisierung der relevanten Aspekte. Zum anderen erzeugen die Methoden ein ganzheitliches Verständnis der Unternehmensstruktur und –prozesse. Somit können alle wesentlichen Faktoren auf konzeptioneller Ebene durchdacht werden. Die Teilnehmer durchlaufen dabei die vier Phasen der Ideengenerierung, Machbarkeitsanalyse, Prototyping sowie Entscheidungsfindung und schließt damit nahtlos an die vorherigen Workshops an. ",
                    ExternalEventUrl = null,
                    Start = new DateTime(2017, 12, 04, 9, 30, 0).ToUniversalTime(),
                    End = new DateTime(2017, 12, 05, 16, 30, 0).ToUniversalTime(),
                    HasTimesSet = true,
                    IsMandatory = true,
                    HeldBy = null,
                    Visibility = Event.EventVisibility.ForAllTeams,
                    Type = Event.EventType.Workshop
                },
                new Event
                {
                    Name = "Workshop Pitch",
                    Address = "Rintheimer Str. 15, 76131 Karlsruhe",
                    Location = "PionierGarage Launchpad",
                    Description = @"Bei einem Pitch bleibt nicht viel Zeit, die eigene Idee zu erläutern. Deswegen kommt es darauf an, eine spannende Botschaft zu senden und die Aufmerksamkeit der Zuhörer zu gewinnen. Egal ob vor Mitstreitern, Investoren oder einer Jury - die Teilnehmer lernen an diesem Tag intensiv die wichtigsten Inhalte für ihr Pitch-Deck und wie sie ihre Kommunikation Schritt für Schritt auf das Wesentliche reduzieren.",
                    ExternalEventUrl = null,
                    Start = new DateTime(2017, 12, 11, 9, 30, 0).ToUniversalTime(),
                    End = new DateTime(2017, 12, 11, 16, 30, 0).ToUniversalTime(),
                    HasTimesSet = true,
                    IsMandatory = true,
                    HeldBy = null,
                    Visibility = Event.EventVisibility.ForAllTeams,
                    Type = Event.EventType.Workshop
                },
                new Event
                {
                    Name = "Midterm",
                    Address = "Kaiserstraße 146, 76133 Karlsruhe",
                    Location = "Knuddels Büro",
                    Description = @"Nach 5 Wochen in denen die Teams sich intensiv mit ihrem Produkt und ihren Kunden auseinandergesetzt haben, ist es an der Zeit eine Zwischenbilanz zu ziehen. Hierfür präsentieren die angehenden Gründer ihre Ergebnisse am 14. Dezember um 19 Uhr im Büro von Knuddels vor der Gründerszene Karlsruhes.
Nach Feedback von einer hochkarätigen Jury werden die 10 Teams von der Jury ausgewählt, die an der zweiten Hälfte von GROW teilnehmen. Die Pitches sind öffentlich. Jeder ist herzlich eingeladen sich diese anzuschauen! ",
                    ExternalEventUrl = "https://www.facebook.com/events/539954286348103/",
                    Start = new DateTime(2017, 11, 14, 19, 0, 0).ToUniversalTime(),
                    End = new DateTime(2017, 11, 14, 22, 0, 0).ToUniversalTime(),
                    HasTimesSet = true,
                    IsMandatory = true,
                    HeldBy = null,
                    Image = Event_images[1],
                    Visibility = Event.EventVisibility.Public,
                    Type = Event.EventType.MainEvent
                },
                new Event
                {
                    Name = "Final",
                    Address = "Gebäude 20.30, KIT, 76131 Karlsruhe",
                    Location = "Kollegiengebäude Mathematik",
                    Description = @"Am 25. Januar findet das Finale des Gründungswettbewerbs statt. Alle zehn verbleibenden Teams pitchen ein letztes Mal vor einer Jury aus fünf bekannten Persönlichkeiten der Karlsruher Gründerszene. Jedem Pitch folgt ein Q&A, beim dem den angehenden Gründern auf den Zahn gefühlt wird.

Anschließend entscheidet die Jury über die drei Gewinner. Bewertet werden die Umsetzung und die Innovativität der Idee, sowie der Gesamtfortschritt in den vergangenen elf Wochen. Es wird Preisgeld im Wert von 5.000€ vergeben, außerdem bekommen die Gewinner die Möglichkeit für bis zu sechs Monate ein Büro in unserem Inkubator und Coworking-Space, dem Launchpad zu beziehen. Das Finale ist eine öffentliche Veranstaltung. Wir freuen uns über zahlreiche Besucher! ",
                    ExternalEventUrl = "https://www.facebook.com/events/155018338475708/",
                    Start = new DateTime(2018, 01, 25, 19, 0, 0).ToUniversalTime(),
                    End = new DateTime(2018, 01, 25, 22, 0, 0).ToUniversalTime(),
                    HasTimesSet = true,
                    IsMandatory = true,
                    HeldBy = null,
                    Image = Event_images[2],
                    Visibility = Event.EventVisibility.Public,
                    Type = Event.EventType.MainEvent
                },
            };
        }

        private static void AddImages()
        {
            Event_images = new[]
            {
                new File
                {
                    Url = "/img/icon/bright-lightbulb.png",
                    Name = "bright-lightbulb.png",
                    AltText = "Kickoff icon",
                    Extension = "png",
                    Category = "events"
                },
                new File
                {
                    Url = "/img/icon/funnel.png",
                    Name = "funnel.png",
                    AltText = "Midterm icon",
                    Extension = "png",
                    Category = "events"
                },
                new File
                {
                    Url = "/img/icon/first-place-medal.png",
                    Name = "first-place-medal.png",
                    AltText = "Final icon",
                    Extension = "png",
                    Category = "events"
                },
            };

            Judge_images = new[]
            {
                new File
                {
                    Url = "/img/2018/jury/bernhard.jpg",
                    Name = "bernhard.jpg",
                    AltText = "The judge Bernhard Janke",
                    Extension = "jpg",
                    Category = "people"
                },
                new File
                {
                    Url = "/img/2018/jury/daniel.jpg",
                    Name = "daniel.jpg",
                    AltText = "The judge Daniel Stammler",
                    Extension = "jpg",
                    Category = "people"
                },
                new File
                {
                    Url = "/img/2018/jury/holger.jpg",
                    Name = "holger.jpg",
                    AltText = "The judge Holger Kujath",
                    Extension = "jpg",
                    Category = "people"
                },
                new File
                {
                    Url = "/img/2018/jury/martin.jpg",
                    Name = "martin.jpg",
                    AltText = "The judge Martin Trenkle",
                    Extension = "jpg",
                    Category = "people"
                },
                new File
                {
                    Url = "/img/2018/jury/michael.jpg",
                    Name = "michael.jpg",
                    AltText = "The judge Michael Kimmig",
                    Extension = "jpg",
                    Category = "people"
                },
                new File
                {
                    Url = "/img/2018/jury/orestis.jpg",
                    Name = "orestis.jpg",
                    AltText = "The judge Orestis Terzidis",
                    Extension = "jpg",
                    Category = "people"
                },
            };

            Mentor_images = new[]
            {
                new File
                {
                    Url = "/img/2018/mentors/andreas_fischer.jpg",
                    Name = "andreas_fischer.jpg",
                    AltText = "The mentor Andreas Fischer",
                    Extension = "jpg",
                    Category = "people"
                },
                new File
                {
                    Url = "/img/2018/mentors/ben_romberg.jpg",
                    Name = "ben_romberg.jpg",
                    AltText = "The mentor Ben Romberg",
                    Extension = "jpg",
                    Category = "people"
                },
                new File
                {
                    Url = "/img/2018/mentors/cecile_heger.jpg",
                    Name = "cecile_heger.jpg",
                    AltText = "The mentor Cecile Heger",
                    Extension = "jpg",
                    Category = "people"
                },
                new File
                {
                    Url = "/img/2018/mentors/frederic_tausch.jpg",
                    Name = "frederic_tausch.jpg",
                    AltText = "The mentor Frederic Tausch",
                    Extension = "jpg",
                    Category = "people"
                },
                new File
                {
                    Url = "/img/2018/mentors/hans_busch.png",
                    Name = "hans_busch.png",
                    AltText = "The mentor Hans Busch",
                    Extension = "jpg",
                    Category = "people"
                },
                new File
                {
                    Url = "/img/2018/mentors/heinz_rothermel.jpg",
                    Name = "heinz_rothermel.jpg",
                    AltText = "The mentor Heinz Rothermel",
                    Extension = "jpg",
                    Category = "people"
                },
                new File
                {
                    Url = "/img/2018/mentors/jannik_nefferdorf.jpg",
                    Name = "jannik_nefferdorf.jpg",
                    AltText = "The mentor Jannik Nefferdorf",
                    Extension = "jpg",
                    Category = "people"
                },
                new File
                {
                    Url = "/img/2018/mentors/jonas_fuchs.jpg",
                    Name = "jonas_fuchs.jpg",
                    AltText = "The mentor Jonas Fuchs",
                    Extension = "jpg",
                    Category = "people"
                },
                new File
                {
                    Url = "/img/2018/mentors/karl_lorey.jpg",
                    Name = "karl_lorey.jpg",
                    AltText = "The mentor Karl Lorey",
                    Extension = "jpg",
                    Category = "people"
                },
                new File
                {
                    Url = "/img/2018/mentors/maja_malovic.jpg",
                    Name = "maja_malovic.jpg",
                    AltText = "The mentor Maja Malovic",
                    Extension = "jpg",
                    Category = "people"
                },
                new File
                {
                    Url = "/img/2018/mentors/manuel_koecher.jpg",
                    Name = "manuel_koecher.jpg",
                    AltText = "The mentor Manuel Köcher",
                    Extension = "jpg",
                    Category = "people"
                },
                new File
                {
                    Url = "/img/2018/mentors/martin_rammensee.jpg",
                    Name = "martin_rammensee.jpg",
                    AltText = "The mentor Martin Rammensee",
                    Extension = "jpg",
                    Category = "people"
                },
                new File
                {
                    Url = "/img/2018/mentors/murat_ercan.jpg",
                    Name = "murat_ercan.jpg",
                    AltText = "The mentor Murat Ercan",
                    Extension = "jpg",
                    Category = "people"
                },
                new File
                {
                    Url = "/img/2018/mentors/nestor_rodriguez.jpg",
                    Name = "nestor_rodriguez.jpg",
                    AltText = "The mentor Nestor Rodriguez",
                    Extension = "jpg",
                    Category = "people"
                },
                new File
                {
                    Url = "/img/2018/mentors/peter_greiner.jpg",
                    Name = "peter_greiner.jpg",
                    AltText = "The mentor Peter Greiner",
                    Extension = "jpg",
                    Category = "people"
                },
                new File
                {
                    Url = "/img/2018/mentors/sebastian_boehmer.jpg",
                    Name = "sebastian_boehmer.jpg",
                    AltText = "The mentor Sebastian Böhmer",
                    Extension = "jpg",
                    Category = "people"
                },
            };

            Team_images = new[]
            {
                new File
                {
                    Url = "/img/2018/teams/accesmed_team.jpg",
                    Name = "accesmed_team.jpg",
                    AltText = "Team photo of Acces Medecins",
                    Extension = "jpg",
                    Category = "teams"
                },
                new File
                {
                    Url = "/img/2018/teams/accessmed.png",
                    Name = "accessmed.png",
                    AltText = "Logo of Acces Medecins",
                    Extension = "png",
                    Category = "teamlogos"
                },
                new File
                {
                    Url = "/img/2018/teams/allopi.png",
                    Name = "allopi.png",
                    AltText = "Logo of AlloPI",
                    Extension = "png",
                    Category = "teamlogos"
                },
                new File
                {
                    Url = "/img/2018/teams/bavest.png",
                    Name = "bavest.png",
                    AltText = "Logo of Bavest",
                    Extension = "png",
                    Category = "teamlogos"
                },
                new File
                {
                    Url = "/img/2018/teams/bavest_team.png",
                    Name = "bavest_team.png",
                    AltText = "Team photo of Bavest",
                    Extension = "png",
                    Category = "teams"
                },
                new File
                {
                    Url = "/img/2018/teams/circle.png",
                    Name = "circle.png",
                    AltText = "Logo of Circle",
                    Extension = "png",
                    Category = "teamlogos"
                },
                new File
                {
                    Url = "/img/2018/teams/circle_team.jpg",
                    Name = "circle_team.jpg",
                    AltText = "Team photo of Circle",
                    Extension = "jpg",
                    Category = "teams"
                },
                new File
                {
                    Url = "/img/2018/teams/gimmickgott.png",
                    Name = "gimmickgott.png",
                    AltText = "Team photo of GimmickGott",
                    Extension = "png",
                    Category = "teams"
                },
                new File
                {
                    Url = "/img/2018/teams/gimmickgott_logo.png",
                    Name = "gimmickgott_logo.png",
                    AltText = "Logo of GimmickGott",
                    Extension = "png",
                    Category = "teamlogos"
                },
                new File
                {
                    Url = "/img/2018/teams/heliopas.svg",
                    Name = "heliopas.svg",
                    AltText = "Logo of HelioPas AI",
                    Extension = "svg",
                    Category = "teamlogos"
                },
                new File
                {
                    Url = "/img/2018/teams/heliopas_team.jpg",
                    Name = "heliopas_team.jpg",
                    AltText = "Team photo of HelioPas AI",
                    Extension = "jpg",
                    Category = "teams"
                },
                new File
                {
                    Url = "/img/2018/teams/kbox.png",
                    Name = "kbox.png",
                    AltText = "Logo of Kbox",
                    Extension = "png",
                    Category = "teamlogos"
                },
                new File
                {
                    Url = "/img/2018/teams/mangolearn.jpg",
                    Name = "mangolearn.jpg",
                    AltText = "Team photo of MangoLearn",
                    Extension = "jpg",
                    Category = "teams"
                },
                new File
                {
                    Url = "/img/2018/teams/mangolearn.png",
                    Name = "mangolearn.png",
                    AltText = "Logo of MangoLearn",
                    Extension = "png",
                    Category = "teamlogos"
                },
                new File
                {
                    Url = "/img/2018/teams/read.png",
                    Name = "read.png",
                    AltText = "Logo of Read!",
                    Extension = "png",
                    Category = "teamlogos"
                },
                new File
                {
                    Url = "/img/2018/teams/secureradiationlab.png",
                    Name = "secureradiationlab.png",
                    AltText = "Logo of SecureRadiationLab",
                    Extension = "png",
                    Category = "teamlogos"
                },
                new File
                {
                    Url = "/img/2018/teams/studentenfutter.png",
                    Name = "studentenfutter.png",
                    AltText = "Logo of StudentenFutter",
                    Extension = "png",
                    Category = "teamlogos"
                },
                new File
                {
                    Url = "/img/2018/teams/studentenfutter_team.jpg",
                    Name = "studentenfutter_team.jpg",
                    AltText = "Team photo of Studentenfutter",
                    Extension = "jpg",
                    Category = "teams"
                },
                new File
                {
                    Url = "/img/2018/teams/tortenglueck.png",
                    Name = "tortenglueck.png",
                    AltText = "Logo of Tortenglück",
                    Extension = "png",
                    Category = "teamlogos"
                },
                new File
                {
                    Url = "/img/2018/teams/wetakehealthcare_team.jpg",
                    Name = "wetakehealthcare_team.jpg",
                    AltText = "Team photo of WeTakeHealthCare",
                    Extension = "jpg",
                    Category = "teams"
                },
                new File
                {
                    Url = "/img/2018/teams/wthc.png",
                    Name = "wthc.png",
                    AltText = "Logo of WeTakeHealthCare",
                    Extension = "png",
                    Category = "teamlogos"
                },
                new File
                {
                    Url = "/img/2018/teams/zircle.png",
                    Name = "zircle.png",
                    AltText = "Logo of Zircle",
                    Extension = "png",
                    Category = "teamlogos"
                },
            };

            Orga_images = new[]
            {
                new File
                {
                    Url = "/img/2018/team/anne.jpg",
                    Name = "anne.jpg",
                    AltText = "The team member Anne Eimer",
                    Extension = "jpg",
                    Category = "people"
                },
                new File
                {
                    Url = "/img/2018/team/antonia.jpg",
                    Name = "antonia.jpg",
                    AltText = "The team member Antonia Lorenz",
                    Extension = "jpg",
                    Category = "people"
                },
                new File
                {
                    Url = "/img/2018/team/chris.jpg",
                    Name = "chris.jpg",
                    AltText = "The team member Christian Wiegand",
                    Extension = "jpg",
                    Category = "people"
                },
                new File
                {
                    Url = "/img/2018/team/dominik.jpg",
                    Name = "dominik.jpg",
                    AltText = "The team member Dominik Doerner",
                    Extension = "jpg",
                    Category = "people"
                },
                new File
                {
                    Url = "/img/2018/team/jasmin.jpg",
                    Name = "jasmin.jpg",
                    AltText = "The team member Jasmin Riedel",
                    Extension = "jpg",
                    Category = "people"
                },
                new File
                {
                    Url = "/img/2018/team/martin.jpg",
                    Name = "martin.jpg",
                    AltText = "The team member Martin Thoma",
                    Extension = "jpg",
                    Category = "people"
                },
            };

            Partner_images = new[]
            {
                new File
                {
                    Url = "/img/2018/partner/entechnon.png",
                    Name = "entechnon.png",
                    AltText = "Logo of the EnTechnon",
                    Extension = "png",
                    Category = "partners"
                },
                new File
                {
                    Url = "/img/2018/partner/fmvc.png",
                    Name = "fmvc.png",
                    AltText = "Logo of First Momentum ventures",
                    Extension = "png",
                    Category = "partners"
                },
                new File
                {
                    Url = "/img/2018/partner/grenke.png",
                    Name = "grenke.png",
                    AltText = "Logo of GRENKE",
                    Extension = "png",
                    Category = "partners"
                },
                new File
                {
                    Url = "/img/2018/partner/gruenderwoche.png",
                    Name = "gruenderwoche.png",
                    AltText = "Logo of the Deutsche Gründerwoche",
                    Extension = "png",
                    Category = "partners"
                },
                new File
                {
                    Url = "/img/2018/partner/karlshochschule.png",
                    Name = "karlshochschule.png",
                    AltText = "Logo of the Karlshochschule",
                    Extension = "png",
                    Category = "partners"
                },
                new File
                {
                    Url = "/img/2018/partner/KGS_transparent.png",
                    Name = "KGS_transparent.png",
                    AltText = "Logo of the KIT Gründerschmiede",
                    Extension = "png",
                    Category = "partners"
                },
                new File
                {
                    Url = "/img/2018/partner/kolibri.png",
                    Name = "kolibri.png",
                    AltText = "Logo of Kolibri Games",
                    Extension = "png",
                    Category = "partners"
                },
                new File
                {
                    Url = "/img/2018/partner/lea_partners.png",
                    Name = "lea_partners.png",
                    AltText = "Logo of LEA Partners",
                    Extension = "png",
                    Category = "partners"
                },
                new File
                {
                    Url = "/img/2018/partner/logo_cyb.png",
                    Name = "logo_cyb.png",
                    AltText = "Logo of the CyberForum",
                    Extension = "png",
                    Category = "partners"
                },
                new File
                {
                    Url = "/img/2018/partner/logo_knuddel_big.png",
                    Name = "logo_knuddel_big.png",
                    AltText = "Logo of Knuddels",
                    Extension = "png",
                    Category = "partners"
                },
                new File
                {
                    Url = "/img/2017/partner/sparkasse.jpg",
                    Name = "sparkasse.jpg",
                    AltText = "Logo of Sparkasse Karlsruhe",
                    Extension = "jpg",
                    Category = "partners"
                },
            };
        }

        private static void AddTeams()
        {

            // Teams 2018

            Teams = new[]
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
                    HasDroppedOut = true
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
                    LogoImage = Team_images[1],
                    TeamPhoto = Team_images[0],
                    MembersAsString = "Fatimata Toure, Amir Akbari",
                    HasDroppedOut = true
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
                    LogoImage = Team_images[2],
                    TeamPhoto = null,
                    MembersAsString = "Daniel David, Christian Fleiner, Arjun Rai Gupta, Marvin Okoh, Brian Sailer",
                    HasDroppedOut = true
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
                    HasDroppedOut = true
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
                    LogoImage = Team_images[3],
                    TeamPhoto = Team_images[4],
                    MembersAsString = "Ramtin Babaei, Pedram Babaei",
                    HasDroppedOut = false
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
                    HasDroppedOut = true
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
                    LogoImage = Team_images[5],
                    TeamPhoto = Team_images[6],
                    MembersAsString = "Alexandre Lehr, Finn von Lauppert, Lukas Wipf, Kai Firschau",
                    HasDroppedOut = false
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
                    HasDroppedOut = true
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
                    HasDroppedOut = true
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
                    HasDroppedOut = true
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
                    HasDroppedOut = true
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
                    HasDroppedOut = true
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
                    LogoImage = Team_images[8],
                    TeamPhoto = Team_images[7],
                    MembersAsString = "Madou Mann, Daniel Hank, Eike Dahle ",
                    HasDroppedOut = false
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
                    HasDroppedOut = true
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
                    LogoImage = Team_images[9],
                    TeamPhoto = Team_images[10],
                    MembersAsString = "Ingmar Wolff, Benno Ommerborn, Vladyslav Shapran",
                    HasDroppedOut = false
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
                    HasDroppedOut = true
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
                    HasDroppedOut = true
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
                    LogoImage = Team_images[11],
                    TeamPhoto = null,
                    MembersAsString = "Saksham Gupta, Ishita Gupta",
                    HasDroppedOut = false
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
                    HasDroppedOut = true
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
                    LogoImage = Team_images[13],
                    TeamPhoto = Team_images[12],
                    MembersAsString = "Danil Fedorovsky, Fabian Illner",
                    HasDroppedOut = false
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
                    HasDroppedOut = true
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
                    HasDroppedOut = true
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
                    LogoImage = Team_images[14],
                    TeamPhoto = null,
                    MembersAsString = "Reyhan Düzgün, David Puljiz",
                    HasDroppedOut = false
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
                    HasDroppedOut = true
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
                    LogoImage = Team_images[15],
                    TeamPhoto = null,
                    MembersAsString = "Aaron Griesbaum, Felix Stengel, Johannes Neumaier, Dominic Kis",
                    HasDroppedOut = false
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
                    LogoImage = Team_images[16],
                    TeamPhoto = Team_images[17],
                    MembersAsString = "Giorgio Groß, Mustafa Cint, Kevin Steinbach, Fabian Wenzel",
                    HasDroppedOut = false
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
                    HasDroppedOut = true
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
                    LogoImage = Team_images[18],
                    TeamPhoto = null,
                    MembersAsString = "Elisabeth Goebel, Tobias Budig, Patrick Theobalt, Leander Märkisch",
                    HasDroppedOut = false
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
                    HasDroppedOut = true
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
                    HasDroppedOut = true
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
                    LogoImage = Team_images[20],
                    TeamPhoto = Team_images[19],
                    MembersAsString = "Peter Krieger, Yannick Pietschmann, Marta Golabek",
                    HasDroppedOut = true
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
                    HasDroppedOut = true
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
                    LogoImage = Team_images[21],
                    TeamPhoto = null,
                    MembersAsString = "Jayesh, Arthur, Enxhi, Andy, Frederik, Mario, Sven",
                    HasDroppedOut = true
                },

                // Teams 2017

                new Team
                {
                    Name = "BestFit",
                    ActiveSince = "Ende '16",
                    Description = "Wir bringen den digitalen Gesundheitstrainer ins Fitnessstudio für Menschen mit Muskel-Skelett-Beschwerden.",
                    Email = "support@bestfitapp.info",
                    WebsiteUrl = "http://bestfitapp.info",
                    FacebookUrl = "BestFitApp",
                    InstagramUrl = null,
                    LogoImage = new File { Url = "/img/2017/teams/bestfit.png", Name = "bestfit.png" },
                    TeamPhoto = new File { Url = "/img/2017/teams/bestfit_team.jpg", Name = "bestfit_team.jpg" },
                    MembersAsString = "Christopher Oertel, Artem Titarenko, Marc Neuhoff ",
                    HasDroppedOut = false
                },
                new Team
                {
                    Name = "Sunshift",
                    ActiveSince = "Okt. '17",
                    Description = "SunShift analysiert Geschäftsdaten und Umweltfaktoren, allen voran das Wetter, und erstellt anhand dieser Parameter einen zuverlässigen Umsatz-Forecast, plant den Personalbedarf und erstellt und verwaltet den Schichtplan für die kommenden Tage.",
                    Email = "dave.meiborg@gmail.com",
                    WebsiteUrl = null,
                    FacebookUrl = null,
                    InstagramUrl = null,
                    LogoImage = new File { Url = "/img/2017/teams/sunshift.png", Name = "sunshift.png" },
                    TeamPhoto = null,
                    MembersAsString = "David Meiborg, Marcel Meckes, Johannes Brand, Tim Rädsch ",
                    HasDroppedOut = false
                },
                new Team
                {
                    Name = "Wir für Karlsruhe",
                    ActiveSince = "Juni '17",
                    Description = "Unsere Mission ist es, die lokale Wirtschaft zu stärken und die sozialen Projekte bei ihrer wichtigen Arbeit zu unterstützen. Des Weiteren wollen wir allen Bürgern die Möglichkeit bieten, beim Einkaufen etwas Gutes für die Region zu tun.",
                    Email = "hallo@wir-fuer.org",
                    WebsiteUrl = "http://wir-fuer.org",
                    FacebookUrl = "wirfuerkarlsruhe",
                    InstagramUrl = null,
                    LogoImage = new File { Url = "/img/2017/teams/wfk.png", Name = "wfk.png" },
                    TeamPhoto = new File { Url = "/img/2017/teams/wfk_team.jpg", Name = "wfk_team.jpg" },
                    MembersAsString = "Moritz Röschl, Sebastian Bayer",
                    HasDroppedOut = false
                },
                new Team
                {
                    Name = "Colusto",
                    ActiveSince = "Juli '17",
                    Description = "Colusto ist die erste Plattform für Wein-Upgrades. Mit unserem Konzept helfen wir Weingütern Rabatte zu vermeiden und somit ihre Marken zu schützen. Gleichzeitig schaffen wir mit unserem Upgrade-Model ein attraktives Angebot für unsere Kunden.",
                    Email = "david.jeggle@colusto.de",
                    WebsiteUrl = "http://www.colusto.de",
                    FacebookUrl = "Colusto",
                    InstagramUrl = null,
                    LogoImage = new File { Url = "/img/2017/teams/colusto.png", Name = "colusto.png" },
                    TeamPhoto = new File { Url = "/img/2017/teams/colusto_team.jpg", Name = "colusto_team.jpg" },
                    MembersAsString = "David Jeggle, Kai Jeggle ",
                    HasDroppedOut = false
                },
                new Team
                {
                    Name = "Usertimes",
                    ActiveSince = "Okt. '17",
                    Description = "Für Unternehmen mit digitalen Produkten die ihre Produktentwicklung vorantreiben ist Usertimes die einfache und schnelle Lösung um extern Nutzertests durchzuführen, bei denen ein passender Methodenmix vorgeschlagen wird, automatisiert durchgeführt und Berichte generiert werden.Im Gegensatz zu klassischen Usertests funktioniert dies auch on Demand komplett online und durch intelligente Systeme unterstützt.",
                    Email = "info@usertimes.io",
                    WebsiteUrl = "http://www.usertimes.io",
                    FacebookUrl = "usertimesHQ",
                    InstagramUrl = null,
                    LogoImage = new File { Url = "/img/2017/teams/usertimes.jpg", Name = "usertimes.jpg" },
                    TeamPhoto = null,
                    MembersAsString = "Jonas Fuchs, Timo Schneider, Dominic Staub, Manuel Hölzlein, Anne Pfeifer, Luc Weinbrecht, Max Beume ",
                    HasDroppedOut = false
                },
                new Team
                {
                    Name = "Flatmates",
                    ActiveSince = "Nov. '17",
                    Description = "Eine digitale Plattform für Studenten weltweit, welche die WG/Mitbewohner-Suche vereinfacht und darüber hinaus das WG-Leben digitalisiert.",
                    Email = "info@flat-mates.com",
                    WebsiteUrl = "www.flat-mates.com",
                    FacebookUrl = "Flatmates-359070784540566",
                    InstagramUrl = null,
                    LogoImage = new File { Url = "/img/2017/teams/flatmates.png", Name = "flatmates.png" },
                    TeamPhoto = new File { Url = "/img/2017/teams/flatmates_team.jpg", Name = "flatmates_team.jpg" },
                    MembersAsString = "Raji Sarhi, Anna Loerzer, Kim Skade, Natalie Cuentas Zavala Ponce, Linus Schweizer ",
                    HasDroppedOut = false
                },
                new Team
                {
                    Name = "Sono",
                    ActiveSince = "Mai '17",
                    Description = "Wir sind ein Team aus Stuttgart und Karlsruhe und entwickeln ein intelligentes Pflaster basierend auf neusten Technologien, welches ein individuelles biometrisches Profil der tragenden Person erhebt und auf Basis der Daten eine zuverlässige Risikoeinschätzung für bestimmte Krankheitsbilder ermöglicht. Dieses wird eine gezielte medikamentöse Behandlung und daraus resultierende Vorbeugung ermöglichen. Zunächst fokussieren wir uns auf das Krankheitsbild der Schlaganfälle, bei denen 80% frühzeitig pathologische Indikatoren aufweisen.",
                    Email = "anna.theresa.schneider@hotmail.com",
                    WebsiteUrl = "http://sonopatch.com",
                    FacebookUrl = null,
                    InstagramUrl = null,
                    LogoImage = new File { Url = "/img/2017/teams/sono.png", Name = "sono.png" },
                    TeamPhoto = null,
                    MembersAsString = "Theresa Schneider, Lukas Findeisen, Hannah Bott ",
                    HasDroppedOut = false
                },
                new Team
                {
                    Name = "YUNU",
                    ActiveSince = "Sep. '17",
                    Description = "Wir entwickeln eine App, die als mobiler Ernährungsberater fungieren soll.",
                    Email = "andre@yunu.coach",
                    WebsiteUrl = "http://www.yunu.coach",
                    FacebookUrl = null,
                    InstagramUrl = null,
                    LogoImage = new File { Url = "/img/2017/teams/yunu.jpg", Name = "yunu.jpg" },
                    TeamPhoto = null,
                    MembersAsString = "André Herecki, Eduard Lichtenwald, Robert Otmar ",
                    HasDroppedOut = false
                },
                new Team
                {
                    Name = "Smartivate",
                    ActiveSince = "März '17",
                    Description = "Smartivate ist ein Online-Marktplatz mit einer „alles aus einer Hand“-Lösung für Smart Home Produkte. Kunden werden über die Nutzen von Smart Home Produkten aufgeklärt. Maßgeschneiderte Lösungen um ihre Häuser intelligent zu machen werden den Kunden dort angeboten. Unser browserbasiertes Virtual-Reality-Haus führt zu einer realitätsnahen Erfahrung von Smart Home Produkten, ihre Funktionalitäten und deren Vorteile. Unser Energieeffizienz- Rechner, zeigt das Einsparungspotenzial, das durch die Verwendung von Smart Home Produkten generiert wird. Der hierfür benötigte Algorithmus wurde während einer Masterarbeit entwickelt und ist daher schwer von Konkurrenten zu kopieren. Darüber hinaus liefern wir maßgeschneiderte Produkt-Lösungen in Abhängigkeit der W ohnfläche, des Heizsystems, der persönlichen Präferenzen und des Budgets, aber auch Installationsdienstleistungen sowie informationstechnische Sicherheitsmaßnahmen.",
                    Email = "hello@smartivate.de",
                    WebsiteUrl = "http://www.smartivate.de",
                    FacebookUrl = "smartivatehome",
                    InstagramUrl = null,
                    LogoImage = new File { Url = "/img/2017/teams/smartivate.jpg", Name = "smartivate.jpg" },
                    TeamPhoto = new File { Url = "/img/2017/teams/smartivate_team.png", Name =" smartivate_team.png" },
                    MembersAsString = "Anand Narasipuram, Jeevan Dasan, Sebastian Dahnert ",
                    HasDroppedOut = false
                },
                new Team
                {
                    Name = "tizer",
                    ActiveSince = "Juli '17",
                    Description = "Terminplanung - Terminorganisation - Terminbuchung ",
                    Email = "tizer@gmx.net",
                    WebsiteUrl = null,
                    FacebookUrl = null,
                    InstagramUrl = null,
                    LogoImage = new File { Url = "/img/2017/teams/tizer.png", Name =" tizer.png" },
                    TeamPhoto = null,
                    MembersAsString = "Dennis Maxelon, Janina Häfner, Patrick Anderer ",
                    HasDroppedOut = true
                },
                new Team
                {
                    Name = "Indus",
                    ActiveSince = "Nov. '17",
                    Description = "Our idea is to provide a market place through digital platform for different community within Germany where they can sell and buy things, which are from their culture and which makes them feel like home. ",
                    Email = "ashetty@karlshochschule.de",
                    WebsiteUrl = null,
                    FacebookUrl = null,
                    InstagramUrl = null,
                    LogoImage = null,
                    TeamPhoto = null,
                    MembersAsString = "Aakash Shetty, Vincent Langet ",
                    HasDroppedOut = true
                },
                new Team
                {
                    Name = "Ridy",
                    ActiveSince = "Mitte '17",
                    Description = "Ridy ist ein Online-Marktplatz für den Fahrradverleih. Bei uns kannst du günstig Tandems, Rennräder, Citybikes und vieles mehr von privaten und professionellen Anbietern mieten.",
                    Email = "hallo@ridy.de",
                    WebsiteUrl = "http://www.ridy.de",
                    FacebookUrl = "ridyapp",
                    InstagramUrl = null,
                    LogoImage = new File { Url = "/img/2017/teams/ridy.png", Name = "ridy.png" },
                    TeamPhoto = new File { Url = "/img/2017/teams/ridy_team.jpg", Name = "ridy_team.jpg" },
                    MembersAsString = "Daniel Sobing, Arseniy Kurynyi ",
                    HasDroppedOut = true
                },
                new Team
                {
                    Name = "Fhtagn",
                    ActiveSince = "Nov '17",
                    Description = "Mit KI kann man tolle Dinge tun, wie (vor allem Cryptowährungen) algorithmisch traden, Code besser kompilieren, bessere KI's entwerfen oder sie direkt verkaufen (AIaaS/SingularityNet/Aquise/...). Der Plan ist, eine Vielzahl von Anwendungen nach ihrer Eignung zu evaluieren, state-of-the-art-Verfahren zu vergleichen und anzuwenden, um Probleme zu lösen, die durch die neue Technologie erst seit kurzem lösbar sind. Idealerweise liegt der Fokus der Entwicklungsarbeit auf dem KI-Verfahren und nicht der Anwendung, welche als möglichst unaufwändig gewählt werden soll.",
                    Email = "chris.hiatt@protonmail.com",
                    WebsiteUrl = null,
                    FacebookUrl = null,
                    InstagramUrl = null,
                    LogoImage = null,
                    TeamPhoto = null,
                    MembersAsString = "Chris Hiatt ",
                    HasDroppedOut = true
                },
                new Team
                {
                    Name = "polunio",
                    ActiveSince = "Juli '17",
                    Description = "Eine platform für digitale Abstimmungen zu aktuellen politischen Themen. Die App soll die politische Einbeziehung der Bürgern erhöhen und politischen Organisationen ermöglichen Meinungsbilder zu selbst ausgewählten Themen leichter zu generieren. ",
                    Email = "hi@polunio.de",
                    WebsiteUrl = "http://www.polun.io",
                    FacebookUrl = "poluniomagick",
                    InstagramUrl = null,
                    LogoImage = new File { Url = "/img/2017/teams/polunio.png", Name = "polunio.png" },
                    TeamPhoto = new File { Url = "/img/2017/teams/polunio_team.png", Name = "polunio_team.png" },
                    MembersAsString = "Alexandru Rinciog, Elias Mühlbrecht ",
                    HasDroppedOut = true
                },
                new Team
                {
                    Name = "GermUni",
                    ActiveSince = "Nov '17",
                    Description = "Aufwändige Bewerbungsprozesse führen zu Verwirrung, Ablehnung von Unis und Enttäuschung für ausländische Studierende. Unsere Lösung dafür ist die Entwicklung eines vollständigen Systems zur Unterstützung des Bewerbungsprozesses für ausländische Studierende an deutschen Universitäten. Wir greifen den Studierenden bei ihrer Bewerbung vom ersten bis zum letzten Schritt ,der gewüschten Zulassung, unter die Arme.",
                    Email = "Germuni.contact@gmail.com",
                    WebsiteUrl = "adornis.de:8080/germuni.com",
                    FacebookUrl = "GermUni-133742043955791",
                    InstagramUrl = null,
                    LogoImage = null,
                    TeamPhoto = null,
                    MembersAsString = "Elham Elahi, Markus Heining, Phelin Guth, Leon Sander ",
                    HasDroppedOut = true
                },
            };
        }

        private static void AddPrizes()
        {
            Prizes = new[]
            {

                // Prizes 2018

                new Prize
                {
                    Name = "1st place",
                    Reward = "3000 € + 6 months at the PionierGarage Launchpad",
                    RewardValue = 3960,
                    Description = null,
                    Winner = Teams.First(t => t.Name == "TortenGlück"),
                    Type = Prize.PrizeType.MainPrize,
                },
                new Prize
                {
                    Name = "2nd place",
                    Reward = "2000 € + 3 months at the PionierGarage Launchpad",
                    RewardValue = 2480,
                    Description = null,
                    Winner = Teams.First(t => t.Name == "Studentenfutter"),
                    Type = Prize.PrizeType.MainPrize,
                },
                new Prize
                {
                    Name = "3rd place",
                    Reward = "1000 € + 1 month at the PionierGarage Launchpad",
                    RewardValue = 1160,
                    Description = null,
                    Winner = Teams.First(t => t.Name == "SecureRadiationLab"),
                    Type = Prize.PrizeType.MainPrize,
                },
                new Prize
                {
                    Name = "Best Product",
                    Reward = "1000 € granted by Kolibri Games",
                    RewardValue = 1000,
                    Description = null,
                    GivenBy = Partners.First(p => p.Name == "Kolibri Games"),
                    Winner = Teams.First(t => t.Name == "TortenGlück"),
                    Type = Prize.PrizeType.SpecialPrize,
                },
                new Prize
                {
                    Name = "Most Scalable",
                    Reward = "1000 € + mentoring through LEA Partners",
                    RewardValue = 1200,
                    Description = null,
                    GivenBy = Partners.First(p => p.Name == "LEA Partners"),
                    Winner = Teams.First(t => t.Name == "HelioPas AI"),
                    Type = Prize.PrizeType.SpecialPrize,
                },
                new Prize
                {
                    Name = "Most Investment-Ready",
                    Reward = "Investment offer by First Momentum Ventures",
                    RewardValue = 0,
                    Description = null,
                    GivenBy = Partners.First(p => p.Name == "First Momentum Ventures"),
                    Winner = Teams.First(t => t.Name == "Read!"),
                    Type = Prize.PrizeType.SpecialPrize,
                },

                // Prizes 2017

                new Prize
                {
                    Name = "1st place",
                    Reward = "3000 € + 6 months at the PionierGarage Launchpad",
                    RewardValue = 3960,
                    Description = null,
                    Winner = Teams.First(t => t.Name == "Usertimes"),
                    Type = Prize.PrizeType.MainPrize,
                },
                new Prize
                {
                    Name = "2nd place",
                    Reward = "1500 € + 3 months at the PionierGarage Launchpad",
                    RewardValue = 1980,
                    Description = null,
                    Winner = Teams.First(t => t.Name == "Colusto"),
                    Type = Prize.PrizeType.MainPrize,
                },
                new Prize
                {
                    Name = "3rd place",
                    Reward = "500 € + 1 month at the PionierGarage Launchpad",
                    RewardValue = 660,
                    Description = null,
                    Winner = Teams.First(t => t.Name == "Sono"),
                    Type = Prize.PrizeType.MainPrize,
                },
                new Prize
                {
                    Name = "Best App",
                    Reward = "500 € granted by Selfbits",
                    RewardValue = 500,
                    Description = null,
                    GivenBy = null,
                    Winner = Teams.First(t => t.Name == "Flatmates"),
                    Type = Prize.PrizeType.SpecialPrize,
                },
                new Prize
                {
                    Name = "Best Market Focus",
                    Reward = "500 € granted by innoWerft",
                    RewardValue = 500,
                    Description = null,
                    GivenBy = null,
                    Winner = Teams.First(t => t.Name == "Usertimes"),
                    Type = Prize.PrizeType.SpecialPrize,
                },
            };
        }
        
        private static ICollection<TEntity> SetIds<TEntity>(this IEnumerable<TEntity> enumerable, int contestId) where TEntity : BaseTimestampedEntity
        {
            var list = new List<TEntity>();
            foreach (var elem in enumerable)
            {
                if (elem is BaseContestSubEntity)
                {
                    (elem as BaseContestSubEntity).ContestId = contestId;
                }
                elem.IsActive = true;
                list.Add(elem);
            }
            return list;
        }
    }
}
