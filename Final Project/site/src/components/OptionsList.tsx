import "./OptionsList.css";

import {
  Slider,
  Typography,
  FormControl,
  Select,
  InputLabel,
  MenuItem,
  Button,
} from "@mui/material";

interface PropsStruct {
  priceRange: Array<number>;
  priceRangeChange: Function;

  sort: string;
  handleSortChange: Function;

  radius: number;
  radiusChange: Function;

  submit: Function;
}

export default function OptionsList(props: PropsStruct) {
  return (
    <div className="options">
      <FormControl variant="standard" sx={{ m: 1, minWidth: 120 }}>
        <InputLabel id="sort-label">Sort by</InputLabel>
        <Select
          labelId="sort-label"
          value={props.sort}
          onChange={props.handleSortChange}
          label="Age"
        >
          <MenuItem value={"rating"}>Rating</MenuItem>
          <MenuItem value={"alphabetical"}>Alphabetical</MenuItem>
          <MenuItem value={"expensive"}>Expensive first</MenuItem>

          <MenuItem value={"opennow"}>Open now first</MenuItem>
          <MenuItem value={"closednow"}>Closed now first</MenuItem>

          <MenuItem value={"cheap"}>Cheapest first</MenuItem>
          <MenuItem value={"distance"}>Closest</MenuItem>
        </Select>
      </FormControl>

      <Typography gutterBottom>Price Range</Typography>
      <Slider
        getAriaLabel={() => "Temperature range"}
        value={props.priceRange}
        onChange={props.priceRangeChange}
        valueLabelDisplay="auto"
        min={0}
        max={5}
      />

      <Typography gutterBottom>Radius</Typography>
      <Slider
        getAriaLabel={() => "Temperature range"}
        value={props.radius}
        onChange={props.radiusChange}
        valueLabelDisplay="auto"
        min={500}
        max={2500}
      />

      <Button variant="contained" onClick={props.submit}>
        Sort
      </Button>
    </div>
  );
}
