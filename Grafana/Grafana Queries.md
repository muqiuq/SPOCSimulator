# Grafana Queries

# SolvedTickets
```sql
Select NOW() as time,
Data.value
From (
(select SolvedTickets as value from summaries where Marker = "$Marker")
union all
values ROW(1) 
union all
(select TotalTickets from summaries where Marker = "$Marker")) Data
```

# Summary
```sql
Select 
CONCAT(TotalCosts," CHF") as TotalCosts, 
CONCAT(FORMAT(TotalWorkingHours,0), " h") as TotalWorkingHours, 
CONCAT(FORMAT(AverageHourlyWage,2), " CHF/h") as AverageHourlyWage
From summaries
Where Marker = "$Marker"
```

# Employees (2nd Level)
```sql
Select 
  time,
  Employees2ndLevelWorking,
  Employees2ndLevelWaiting
FROM datapoints
Where Marker = "$Marker"
ORDER BY time
```

# Employees (1st Level)
```sql
Select 
  time,
  Employees1stLevelWorking,
  Employees1stLevelWaiting
FROM datapoints
WHERE Marker = "$Marker"
ORDER BY time
```

# Tickets (Average times)
```sql
SELECT
  time,
  AverageTicketWaitTime,
  AverageTicketSolveTime,
  AverageTicketSolveTime1stLevel,
  AverageTicketSolveTime2ndLevel
FROM datapoints
WHERE Marker = "$Marker"
ORDER BY tick
```

# Tickets (Count)
```sql
  SELECT
    time,
    DeployedUnsolvedTickets,
    OpenTickets AS "OpenTickets",
    Open2ndLevelTickets,
    Ongoing1stLevelTickets,
    Ongoing2ndLevelTickets
  FROM datapoints
  WHERE Marker = "$Marker"
  ORDER BY tick
```

