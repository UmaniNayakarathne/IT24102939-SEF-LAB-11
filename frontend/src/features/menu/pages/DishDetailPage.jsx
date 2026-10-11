import { useParams, Link } from "react-router-dom";

function DishDetailPage() {
  const { id } = useParams();

  return (
    <div>
      <h2>Dish Details</h2>

      <p>Showing details for dish #{id}</p>

      <Link to="/">← Back to Menu</Link>
    </div>
  );
}

export default DishDetailPage;