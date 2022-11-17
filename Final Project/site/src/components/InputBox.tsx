import { Alert, Button, TextField, Typography, Slider } from "@mui/material";
import TouchRipple from "@mui/material/ButtonBase/TouchRipple";

import { useRef } from "react";

interface PropsStruct {
  range: number;
  updateRange: Function;

  keyword: string;
  updateKeyword: Function;

  submit: VoidFunction;

  errorText: string;
}

import "./InputBox.css";

export default function InputBox(props: PropsStruct) {
  const rippleRef = useRef(null);
  const buttonRef = useRef(null);

  const handleKeyPress = (e: { key: string }) => {
    if (e.key == "Enter") {
      props.submit();

      const input = document.getElementById("keyword_input");

      if (input) {
        input.blur();
      }

      if (rippleRef !== null && buttonRef !== null) {
        const rect = buttonRef.current.getBoundingClientRect();
        rippleRef.current.start(
          {
            clientX: rect.left + rect.width / 2,
            clientY: rect.top + rect.height / 2,
          },
          // when center is true, the ripple doesn't travel to the border of the container
          { center: false }
        );

        setTimeout(() => rippleRef.current.stop({}), 320);
      }
    }
  };

  return (
    <div className="main-box">
      <h1 className="header">Find new great food</h1>
      <p className="slogan">From the comfort of your couch</p>

      <div className="inputs-box">
        {props.errorText !== "" ? (
          <Alert severity="error">{props.errorText}</Alert>
        ) : (
          <></>
        )}

        <TextField
          id="keyword_input"
          variant="standard"
          label="Keyword / Search"
          value={props.keyword}
          onChange={props.updateKeyword}
          color="primary"
          onKeyDown={handleKeyPress}
        />

        <div className="range">
          <Typography>Range (meter)</Typography>
          <Slider
            aria-label="Range"
            value={props.range}
            min={500}
            max={5000}
            onChange={props.updateRange}
            valueLabelDisplay="auto"
          />
        </div>

        <Button onClick={props.submit} ref={buttonRef} variant="contained">
          Search
          <TouchRipple ref={rippleRef} center />
        </Button>
      </div>
    </div>
  );
}
