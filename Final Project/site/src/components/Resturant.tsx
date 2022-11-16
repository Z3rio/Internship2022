import Rating from "@mui/material/Rating";
import Chip from "@mui/material/Chip";

import "./Resturant.css";

interface PropsStruct {
  idx: number;

  el: {
    name: string;
    rating: number;
    types: Array<string>;
    vicinity: string;
    opening_hours: {
      open_now: boolean;
    };
  };
}

function FormatChipString(str: string): string {
  return str.charAt(0).toUpperCase() + str.slice(1).replace(/_/g, " ");
}

export default function Resturant(props: PropsStruct) {
  return (
    <div
      key={props.idx}
      className={
        props.el.opening_hours !== undefined
          ? props.el.opening_hours.open_now == true
            ? "resturant open"
            : "resturant closed"
          : "resturant no-data"
      }
    >
      <div className="upper-text">
        <h1>{props.el.name}</h1>
        <h3>{props.el.vicinity}</h3>
      </div>

      <div className="chips">
        {props.el.types.map((str: string, idx: number) => {
          return (
            <Chip
              label={FormatChipString(str)}
              variant="outlined"
              key={idx}
              size="small"
            />
          );
        })}
      </div>

      <Rating value={props.el.rating} readOnly />
    </div>
  );
}
