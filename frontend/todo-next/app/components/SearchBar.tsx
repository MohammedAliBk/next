
import { Moon,Search} from "lucide-react";


function SearchBar() {
  return (
    <div className="flex items-center gap-3 mt-6">
        {/* Input search bar */}
        <div className="relative w-96 group">
          <Search className="absolute right-3 top-1/2 -translate-y-1/2 text-gray-400 group-focus-within:text-purple-500" />
                <input
          placeholder="Search note..."
          className="w-96 px-4 py-2 rounded-lg border border-purple-400 focus:outline-none focus:ring-2 focus:ring-purple-500"
                />
        </div>
      {/* label to filter tasks */}
      <label htmlFor="Tasks" className="bg-purple-700 text-white px-4 py-2 rounded-lg">
        <select id="tasks" name="tasks">
          <option value="All">All</option>
          <option value="Complete">Complete</option>
          <option value="Incomplete">Incomplete</option>
        </select>
      </label>

      {/* Dark mode button */}
      <button className="bg-purple-700 text-white p-2 rounded-lg shadow-lg hover:scale-110 transition"><Moon/></button>
    </div>
  );
}

export default SearchBar;
