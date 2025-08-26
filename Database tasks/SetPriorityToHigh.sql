UPDATE TodoItems
SET Priority = 3
WHERE Priority <> 3 AND CreatedAt < '2025.08.27' AND IsDone = 0