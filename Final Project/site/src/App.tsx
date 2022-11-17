import { useState } from "react";
import "./App.css";

import Swal from "sweetalert2";
import withReactContent from "sweetalert2-react-content";

import axios from "axios";

import InputBox from "./components/InputBox";
import Resturant from "./components/Resturant";
import NavBar from "./components/NavBar";
import OptionsList from "./components/OptionsList";

import { ThemeProvider, createTheme } from "@mui/material/styles";

const SweetAlert = withReactContent(Swal);

const darkTheme = createTheme({
  palette: {
    mode: "dark",
  },
});

interface ResturantStruct {
  name: string;
  rating: number;
  types: Array<string>;
  vicinity: string;
  permanently_closed: boolean;
  business_status: string;
  opening_hours: {
    open_now: boolean;
  };
}

let cooldown = false;

function App() {
  const [resturants, setResturants] = useState<Array<ResturantStruct>>([]);
  const [errorText, setErrorText] = useState<string>("");

  const [range, setRange] = useState<number>(1000);
  const [keyword, setKeyword] = useState<string>("");

  const [loggedIn, setLoggedIn] = useState<boolean>(true);

  const [priceRange, setPriceRange] = useState<Array<number>>([0, 5]);
  const [sort, setSort] = useState<string>("rating");

  const [onlyOpenNow, setOnlyOpenNow] = useState<boolean>(false);

  function updateRange(e: any): void {
    setRange(e.target.value);
  }

  const handlePriceRangeChange = (event: Event, newValue: number[]) => {
    setPriceRange(newValue);
  };

  const handleSortChange = (
    event: Event,
    newValue: { props: { value: string } }
  ) => {
    setSort(newValue.props.value);
  };

  const handleOnlyOpenNowChange = (event: Event, newValue: boolean) => {
    setOnlyOpenNow(newValue);
  };

  const handleRadiusChange = (event: Event, newValue: number) => {
    setRange(newValue);
  };

  function updateKeyword(e: any): void {
    setKeyword(e.target.value);
  }

  function submit(): void {
    if (cooldown == false) {
      if (keyword == "") {
        SweetAlert.fire({
          title: "Invalid keyword",
          text: "You have to input an keyword before trying to search",
          icon: "error",
        });
        setResturants([]);
        return;
      }

      if (range < 500 || range > 5000) {
        SweetAlert.fire({
          title: "Invalid range",
          icon: "error",
        });
        setResturants([]);
        return;
      }

      axios
        .post(
          `https://localhost:7115/resturants/search?search=${keyword}&radius=${range}&minPrice=${priceRange[0]}&maxPrice=${priceRange[1]}&sort=${sort}&onlyOpenNow=${onlyOpenNow}`
        )
        .then((resp: any) => {
          cooldown = true;
          if (resp.status == 200) {
            if (resp.data.status !== "ZERO_RESULTS") {
              setResturants(resp.data.results);
              setErrorText("");
            } else {
              setResturants([]);
              setErrorText("No search results found...");
            }
          } else {
            setResturants([]);
            console.error(resp.statusText);
            setErrorText(resp.statusText);
          }
        })
        .catch((err: any) => {
          if (err) {
            setResturants([]);
            console.error(err);
          }
        });
      setTimeout(function () {
        cooldown = false;
      }, 2500);
    } else {
      SweetAlert.fire({
        title: "You have to wait",
        text: "You have to wait before making a new search",
        icon: "error",
      });
    }
  }

  return (
    <div className="App">
      <NavBar loggedIn={loggedIn} setLoggedIn={setLoggedIn} />

      <ThemeProvider theme={darkTheme}>
        <InputBox
          errorText={errorText}
          range={range}
          updateRange={updateRange}
          keyword={keyword}
          updateKeyword={updateKeyword}
          submit={submit}
        />
      </ThemeProvider>

      {resturants.length > 0 ? (
        <div className="info-cont">
          <div className="resturants">
            {resturants.map((el: ResturantStruct, idx: number) => {
              return el.permanently_closed !== true &&
                el.business_status == "OPERATIONAL" ? (
                <Resturant key={idx} el={el} idx={idx} />
              ) : (
                ""
              );
            })}
          </div>

          <OptionsList
            priceRange={priceRange}
            priceRangeChange={handlePriceRangeChange}
            sort={sort}
            handleSortChange={handleSortChange}
            radius={range}
            radiusChange={handleRadiusChange}
            submit={submit}
            onlyOpenNow={onlyOpenNow}
            onlyOpenNowChanged={handleOnlyOpenNowChange}
          />
        </div>
      ) : (
        <></>
      )}
    </div>
  );
}

export default App;
