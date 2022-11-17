import Rating from "@mui/material/Rating";
import Chip from "@mui/material/Chip";
import { styled } from "@mui/material/styles";

import "./Resturant.css";

import AttachMoneyIcon from "@mui/icons-material/AttachMoney";
import AttachMoneyOutlinedIcon from "@mui/icons-material/AttachMoneyOutlined";

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

const StyledRating = styled(Rating)({
  "& .MuiRating-iconFilled": {
    color: "#118C4F",
  },
  "& .MuiRating-iconHover": {
    color: "#118C4F",
  },
});

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

      <div className="ratings">
        <Rating value={props.el.rating} precision={0.1} readOnly />

        <StyledRating
          className="money"
          icon={<AttachMoneyIcon fontSize="inherit" />}
          emptyIcon={<AttachMoneyOutlinedIcon fontSize="inherit" />}
          readOnly
          precision={0.1}
          value={props.el.rating}
        />
      </div>
    </div>
  );
}
