import Slider from "@mui/material/Slider";
import Typography from "@mui/material/Typography";
import TextField from "@mui/material/TextField";
import Button from "@mui/material/Button";
import Alert from "@mui/material/Alert";

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
  return (
    <div className="input-box">
      {props.errorText !== "" ? (
        <Alert severity="error">{props.errorText}</Alert>
      ) : (
        <></>
      )}

      <TextField
        variant="standard"
        label="Keyword / Search"
        value={props.keyword}
        onChange={props.updateKeyword}
        color="primary"
      />

      <div>
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

      <Button onClick={props.submit} variant="outlined">
        Search
      </Button>
    </div>
  );
}
