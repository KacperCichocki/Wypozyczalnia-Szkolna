# Wypożyczalnia sprzętu szkolnego

Aplikacja desktopowa służąca do zarządzania wypożyczaniem i zwracaniem sprzętu szkolnego. Projekt powstał w celu uproszczenia ewidencji wyposażenia oraz monitorowania jego dostępności.

## Status projektu

**MVP – Minimum Viable Product**

Pierwsza wersja aplikacji udostępnia podstawowe funkcjonalności potrzebne do obsługi wypożyczalni sprzętu szkolnego.

## Technologie

- C#
- .NET
- WPF (Windows Presentation Foundation)
- XAML

## Funkcjonalności

- Wyświetlanie listy dostępnego sprzętu.
- Przeglądanie informacji o sprzęcie:
  - nazwa,
  - kategoria,
  - numer seryjny,
  - stan techniczny,
  - status dostępności.
- Wyszukiwanie sprzętu po nazwie, kategorii i numerze seryjnym.
- Wypożyczanie pojedynczych przedmiotów.
- Wypożyczanie wielu przedmiotów jednocześnie.
- Zwracanie pojedynczych przedmiotów.
- Zwracanie wielu przedmiotów jednocześnie.
- Przypisywanie sprzętu do osoby wypożyczającej.
- Rejestrowanie operacji wypożyczenia i zwrotu w historii.
- Przeglądanie historii operacji.

## Uruchomienie

1. Sklonuj repozytorium:

   ```bash
   git clone https://github.com/KacperCichocki/Wypozyczalnia-Szkolna/tree/mvp
   ```

2. Otwórz rozwiązanie projektu w Visual Studio.
3. Przywróć wymagane zależności NuGet, jeśli jest to konieczne.
4. Uruchom projekt za pomocą przycisku **Start** lub klawisza `F5`.

## Przechowywanie danych

W obecnej wersji dane przechowywane są w pamięci aplikacji.

- Lista sprzętu oraz historia operacji są przechowywane w kolekcjach obiektów.
- Dane nie są zapisywane w zewnętrznej bazie danych.
- Po zamknięciu aplikacji wprowadzone zmiany zostają utracone.

## Ograniczenia MVP

- Brak bazy danych.
- Brak trwałego zapisu danych.
- Brak systemu kont użytkowników i logowania.
- Brak zaawansowanego raportowania.
- Brak obsługi wielu stanowisk jednocześnie.

## Plany rozwoju

- Implementacja bazy danych.
- Trwałe przechowywanie sprzętu i historii wypożyczeń.
- Rozbudowa zarządzania użytkownikami.
- Dodanie możliwości dodawania, edytowania i usuwania sprzętu.
- Rozbudowa historii oraz filtrowania operacji.
- Dalsze ulepszanie interfejsu użytkownika.

## Cel projektu

Celem projektu jest stworzenie prostego i intuicyjnego narzędzia wspomagającego zarządzanie szkolnym wyposażeniem, ograniczającego konieczność ręcznego prowadzenia ewidencji wypożyczeń i zwrotów.