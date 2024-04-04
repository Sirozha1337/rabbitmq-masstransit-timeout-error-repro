Running a Publisher with RabbitMQ transport fails publishing all messages except one when connection times out.

Steps to reproduce (using my example project):
1. Run RabbitMQ inside a docker container, Consumer and Publisher
2. Write something in Publisher console and hit enter
3. See that both messages are received by a consumer
4. Now use `docker pause rabbitmq` to imitate host shutdown
5. Write something in Publisher console and hit enter
6. Wait for approximately two minutes
7. You will see an error printed in the Publisher console
8. Now use `docker unpause rabbitmq`
9. See that consumer only received message with `From Publisher1:` prefix