const { App } = require("@slack/bolt");
require("dotenv").config();

const https = require("https")

const app = new App({
    token: process.env.SLACK_BOT_TOKEN,
    signingSecret: process.env.SLACK_SIGNING_SECRET,
    socketMode:true,
    appToken: process.env.APP_TOKEN
});

const httpsAgent = new https.Agent({
    rejectUnauthorized: false,
});

process.env.NODE_TLS_REJECT_UNAUTHORIZED = "0";

app.command("/searchresturant", async ({ command, ack, say }) => {
    try {
        await ack();
        const args = command.text.split(" ")

        if (args.length !== 2) {
            say("Invalid parameters")
        } else {
            const keyword = args[0]
            const radius = args[1]

            fetch(`https://localhost:7115/resturants/search?search=${keyword}&radius=${radius}`, {
                method: "GET",
                agent: httpsAgent,
            }).then(async (resp) => {
                const data = JSON.parse(await resp.text())
                let resturants = [
                    {
                        "type": "header",
                        "text": {
                            "type": "plain_text",
                            "text": "Resturant search"
                        }
                    },
                    {
                        "type": "section",
                        "text": {
                            "type": "plain_text",
                            "text": `You searched for "${keyword}" and got ${data.results.length}x results`,
                            "emoji": true
                        }
                    },
                    {
                        "type": "divider"
                    }
                ]

                for (let i = 0; i < data.results.length; i++) {
                    const resturant = data.results[i]

                    resturants.push({
                        type: "section",
                        open_now: resturant.opening_hours.open_now,
                        "fields": [{
                            type: "mrkdwn",
                            text: `*${resturant.name}* - \n${resturant.opening_hours.open_now == true ? "✅" : "❌"} ${resturant.vicinity}`
                        }]
                    })
                }

                resturants.sort(function(a,b) {
                    return b.open_now - a.open_now
                })

                for (let i = 0; i < resturants.length; i++) {
                    delete resturants[i].open_now;
                }

                say({
                    "blocks": resturants
                })
            })
            .catch((err) => {
                if(err) {
                    console.error(err)
                }
            })
        }
    } catch (error) {
        console.log("err")
      console.error(error);
    }
});

(async () => {
    const port = 3000
    await app.start(process.env.PORT || port);
    console.log(`⚡️ Slack Bolt app is running on port ${port}!`);
})();