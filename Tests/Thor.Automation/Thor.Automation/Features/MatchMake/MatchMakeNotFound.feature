@matchMake
Feature: MatchMade not found when only one player on queue
	
Background: 
	Given a clean queue

@notfound
Scenario: Match not found
	Given a player with elo 1200
	When the player searches for a match on the queue
	Then the player should receive a 404 status code

@notfound 
Scenario: Match not found for elo in the range higher/lower (-200, +200)
	Given a player with elo <player1>
	When the player searches for a match on the queue
	Then the player should receive a 404 status code
	Given a player with elo <player2>
	When the player searches for a match on the queue
	Then the player should receive a 404 status code
	And queue should not be empty

Examples: 
  | player1 | player2 |
  | 1200    | 1401    |
  | 1300    | 1501    |
  | 1400    | 1601    |
  | 1300    | 1800    |
  | 1500    | 1299    |
  | 1500    | 2500    |
 
@notfound @error
Scenario: Player already on queue with lock. second request should fail
	Given a player with elo 1000
	When the player sends two concurrent requests for a match on the queue
	Then the player should receive a 400 status code

@found
Scenario: Match found
	Given a player with elo <player1>
	When the player searches for a match on the queue
	Then the player should receive a 404 status code
	Given a player with elo <player2>
	When the player searches for a match on the queue
	Then the player should receive a 200 status code
	And queue should be empty
Examples:
	|player1| player2|
	|1200   | 1300   |
	|1200   | 1200   |
	|1200   | 1100   |
	|1500   | 1500   | 
	|1500   | 1600   |
	|1000   | 1100   |
 
	

