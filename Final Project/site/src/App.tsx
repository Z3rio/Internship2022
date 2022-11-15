import { useState } from "react";
import "./App.css";

import Swal from "sweetalert2";
import withReactContent from "sweetalert2-react-content";

import axios from "axios";

import Button from "@mui/material/Button";

import InputBox from "./components/InputBox";
import Resturant from "./components/Resturant";

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

function App() {
  const [resturants, setResturants] = useState([]);
  const [errorText, setErrorText] = useState("");

  const [range, setRange] = useState(1000);
  const [keyword, setKeyword] = useState("");

  function updateRange(e: any): void {
    setRange(e.target.value);
  }

  function updateKeyword(e: any): void {
    setKeyword(e.target.value);
  }

  function submit() {
    if (keyword == "") {
      SweetAlert.fire({
        title: "Invalid keyword",
        text: "You have to input an keyword before trying to search",
        icon: "error",
      });
      return;
    }

    if (range < 500 || range > 5000) {
      SweetAlert.fire({
        title: "Invalid range",
        icon: "error",
      });
      return;
    }

    axios
      .post(
        `https://localhost:7115/resturants?search=${keyword}&radius=${range}`
      )
      .then((resp: any) => {
        if (resp.status == 200) {
          if (resp.data.status !== "ZERO_RESULTS") {
            setResturants(resp.data.results);
            setErrorText("");
          } else {
            setErrorText("No search results found...");
          }
          console.log(resp);
        } else {
          console.error(resp.statusText);
          setErrorText(resp.statusText);
        }
      })
      .catch((err: any) => {
        if (err) {
          console.error(err);
        }
      });
  }

  function goBack() {
    setResturants([]);
  }

  return (
    <div className="App">
      <ThemeProvider theme={darkTheme}>
        <div className="header">
          <h1>Food Searcher</h1>
          <p>This is the best website ever to cure your hunger</p>
        </div>

        {resturants.length == 0 ? (
          <InputBox
            errorText={errorText}
            range={range}
            updateRange={updateRange}
            keyword={keyword}
            updateKeyword={updateKeyword}
            submit={submit}
          />
        ) : (
          <Button variant="outlined" className="goback-button" onClick={goBack}>
            Go Back
          </Button>
        )}

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
      </ThemeProvider>
    </div>
  );
}

export default App;
