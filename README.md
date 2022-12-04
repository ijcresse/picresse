p0: add hint/mark button for counting. total: 5
 add button 1
 add color fill 2
  interaction: only fills empty square 1
  overwritten by anything 2
  cleared by hitting empty again 1
 ensure doesn't interfere with puzzle solving, just colors square 2-3
 
p1: create puzzle 8
 'draw puzzle' button on main menu 2
 new draw puzzle view 5
  use current puzzle view 1
  change functionality to support puzzle creation 3
   on fly puzzle code generation 1
   only fill grid, no need to alert puzzle or anything 3
   fill and clear only, no cross/hint 1
   
   +resize functionality (wipes progress) NOT MVP 5
    menu button brings up options 3
    add option to change size of current puzzle 2
     alert to ensure user doesn't accidentally wipe progress 2

p2: puzzle archive
FE 21
 view puzzle list
  add puzzle box at top 3
   puzzle name, puzzle code 1
   [auth] puzzle creator automatically attached 1
   refresh page on successful puzzle submission 2
  list of puzzles. 8
   paginated. 50 per page (?) 3
   sortable by headers 5
    puzzle id, puzzle name, creator, date created, width, length
    buttons: completed, warn, delete 3
     puzzle name revealed after hitting COMPLETED button 1
     puzzle greys out on warn (this user? all?) 2
     [auth] puzzle delete button only shows for puzzle creator 2
    puzzle code given to user by clicking on row 1
     popover - copied to clipboard 1
  search puzzle tool. default search just loads everything 3
   search by header 3
    id, name, creatorId, width, length, puzzleCode, hideCompleted
  alert system for bad requests, inauthenticated sessions (requiring re-logging in), successful puzzle submission, etc.

 authenticated page 5
  investigate how to integrate logins
   npx http-server
    landing page requires login, login button checks against forum
    register button sends you to forum registration. forget and all that sends you to forum login.
    get rid of cookie warning on forum. who cares.

BE 13
 figure out what exactly is running this 2
 startup flow 3
  maintain connection to puzzleDB and forumDB
 loginRequest 3
  open forumDB connection, see if auth is correct. form userData obj and pass it back to user. close forumDB connection.
 ensure userinfo is appended to each request, or else return forbidden 3-5. need to remember how this works.
 get puzzle list (pagesize, offset) 3
  bare request gives paginated results of everything
  get puzzle list (pagesize, offset, puzzlesearchrequestparams) 2
   append strings for db query based on params
 add puzzle (puzzle name, puzzle code) 2
  insert into db if not exists
 update puzzle (puzzle info) 2
  for warning, or flagging puzzle as completed for user

DB 3
 create puzzleDB 3
  fields: *puzzleId, *puzzleName, *puzzleCode, *puzzleCreator, *dateCreated, *width, *length, dateUpdated, dateDeleted, isDeleted, isWarned, timesCompleted
  * denotes searchable fields
 separate from forumDB

