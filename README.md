# LatencyMonitor

## Description

Applications that use cloud-based database architecture are at risk of data loss in the event of database outage.
Azure’s current mechanism for ensuring continuity of service and disaster recovery in the event of outage is geo-
replication. One or multiple active copies of the primary database are maintained in a different geographical
location to the primary database; primary outage results in forced failover to the secondary database, if auto
failover is configured. Microsoft documentation concedes that this kind of failover “will result in data loss”, which
implies some degree of replication latency. This project produces an application that monitors the degree of
latency between primary and secondary databases, allowing business users to assess the risk of data loss in the
event of outage. There are many sophisticated tools for monitoring local replication using SQL Server Always On
technology, but there are not so many established technologies for geo-replication.

## Design

The figure below highlights the structure of the client code of the application. The central object to the system is
the Database. The central design pattern to be employed is the observer pattern. In this design template the
database acts as a publisher and has many subscribers.

![alt text](/path/img.jpg "Title")

Pollers connect, query and update the status of each database, which must publish events to any subscribers
interested in this update. To facilitate this pattern the database class will maintain a static observable collection of
all databases in the system that broadcasts events whenever its members are changed.
A generalised abstract subscriber class allows an extensible subscriber model, where new UI subscribers may be
implemented to meet new requirements.
A factory pattern is employed to read the MonitorConfig.xml file to construct and group databases, then associate
these replication groups with UI elements and event bindings. This dependency injection promotes data
independence and flexibility for handling many group configurations.

## Use

This latency monitor allows users to observe the status of any failover group, plus various measurements of
latency between members. The user may start and stop polling each group to receive the current status. The user
is notified of the most severe problem within a failover group, when latency readings fall outside certain defined
boundaries. Also, current failover group configurations are presented as a dynamic map.
It is possible to test the resilience of any failover group deployment to high traffic, by using the inbuilt writer
functionality. This sends traffic at a user defined rate to the primary database.
To monitor a set of failover group deployments in this application you must edit the MonitorConfig.xml file to
describe your deployments before building and publishing. Once published it can be installed and run as a ‘Click-
Once’ application. A user would have to set up their own database deployment, edit the configuration file and permit their IP address in Azure portal.
