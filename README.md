# Office Simulation

A Unity simulation of an office where workers autonomously manage their own needs and generate revenue for the company. The goal was never a polished game — it was to watch emergent chaos unfold as individual workers, each with their own personality, make decisions that occasionally conflict with everyone else's.

## What It Does

Workers spawn into the office and navigate between stations — desks, a breakroom, bathrooms, and the CEO's office. Each worker earns money for the company while working at their desk, but over time their stats degrade and they need to address them. When a stat drops below their personal threshold, they leave their desk and go sort it out.

The result is a small simulation of an unoptimised office: people constantly getting up, getting in each other's way, and occasionally all deciding they need a break at the same time.

## The Stat System

Each worker has four stats: stress, motivation, fun, and exhaustion. What makes them behave differently from each other is that both the threshold at which each stat triggers a response, and the priority order in which stats are checked, are randomised at spawn time.

This was a deliberate design choice. Real people have different tolerances and different things they prioritise. One worker might push through exhaustion but abandon their desk the moment stress gets high. Another might ignore everything until their motivation collapses. The randomised priority order means no two workers behave identically even in the same situation.

## Technical Implementation

- `Station` is a base class extended by `WorkStation`, `Bathroom`, `Breakroom`, and `Ceo` — each overrides `DoTask()` to apply different stat effects to the worker using it
- Workers use Unity's NavMesh system for navigation and evaluate their stats on a configurable tick rate rather than every frame
- Station slot assignment uses a claim-on-query system to prevent multiple workers being sent to the same position simultaneously
- Worker state (travelling, working, idle) is managed internally with a task counter that triggers re-evaluation after a configurable number of completed tasks

## What I Would Do Differently

The position assignment and queuing logic is the weakest part of the project — workers occasionally compete for the same destination, which causes navigation conflicts. I would replace the current random-station-search with a proper queuing system, and increase station stopping distances to give agents more physical space.

I would also make the office itself dynamic — as more workers are hired, the office grows, new desks appear, and the player makes decisions about where to invest the money being generated. The foundation for that loop is already here; the GameManager handles spawning and money, it just never got the UI to go with it.

## Built With

- Unity / C#
- Unity NavMesh for agent navigation
- Solo project — AI Coursework, Goldsmiths University of London, BSc Games Programming
