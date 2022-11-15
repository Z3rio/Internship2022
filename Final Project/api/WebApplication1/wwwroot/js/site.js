const viewmodel = Vue({
    data() {
        return {

        }
    }
})

let radius = 2000
let keyword = ""
let resturants = []

let debounce = false

const resturantsEl = document.getElementById("resturants")
const input = document.getElementById("input")
const range = document.getElementById("slider")
const btn = document.getElementById("searchBtn")
const errorText = document.getElementById("errorText")

function updateFetch() {
    if (debounce == false) {
        debounce = true

        fetch(`https://localhost:7115/resturants?search=${keyword}&radius=${radius}`).then(async (resp) => {
            if (resp.ok) {
                let json = await resp.text()

                if (json) {
                    resturants = JSON.parse(json).results
                }

                if (resturants) {
                    resturantsEl.innerHTML = ""

                    for (i = 0; i < resturants.length; i++) {
                        const data = resturants[i]

                        resturantsEl.innerHTML = `${resturantsEl.innerHTML}
<div class="resturant">
    <h1>${data.name}</h1>
    ${data.opening_hours !== undefined ? `<h2 style="color:${data.opening_hours.open_now ? "green" : "red"}">${data.opening_hours.open_now ? "Open right now" : "Closed right now"}` : "<h2>No open data found</h2>"}
</div>
`
                    }
                }
            }
        }).catch((err) => {
            if (err) {
                console.error(err)
            }
        })

        setTimeout(function () {
            debounce = false
        }, 2500)
    }
}


if (input) {
    function inputHandler(e) {
        keyword = e.target.value
    }

    input.addEventListener("propertyChange", inputHandler)
    input.addEventListener("input", inputHandler)
}


if (range) {
    function rangeHandler(e) {
        radius = e.target.value
        console.log(radius)
    }

    range.addEventListener("propertyChange", rangeHandler)
    range.addEventListener("input", rangeHandler)
}


if (btn) {
    btn.addEventListener("click", (e) => {
        updateFetch()
    })
}