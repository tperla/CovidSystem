# Covid System

## Description

1. The project was built with Sql because a relational DB allows for more convenient retrieval of records, and from my familiarity with EntityFramework.
2. In the project I used EntityFramwork which enables the mapping of the Db tables to objects in the code, and actually enables the management of all queries and searches in a more convenient way through the code.
3. To allow the project to run independently of any installations, I used docker to wrap my-sql and allow the api to work with it.
4. The api itself also runs inside docker and to synchronize the 2 running components I used docker-compose which allows several containers to be run together.
5. To enable convenient testing, I used postman which is a tool for api testing.

## How to run the code
1. Download the project from git.
2. Navigate in cmd to the project folder where it was installed locally, and run docker compose up (make sure that docker is installed on the station where you are running, in addition to paying attention to the folders where the docker compose file is located)
3. You can debug the api through vs by pressing f5 and see the api in swagger.
4. To enable different types of tests, download the CovidSystems file found in the Postman_Tests folder and import it in Postman.
5. First details should be entered into the DB in the following order:
    1. Manufacturers
    2. Member
    3. Vaccinations
After that you can run the tests.
6. In addition I added the ability to upload an image, the image is saved in the DB as base64 to save space.
7. The charts are in the Design folder.
8. I added the ability to output data in a summary view, it also appears as part of the set of tests in Postman.
9. I added a Requirements and Tests Review document for the Group Isolation feature, found under the QA folder.


....
