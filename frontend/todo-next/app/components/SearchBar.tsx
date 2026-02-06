function SearchBar() {
  return (
    <div className="flex items-center gap-3 mt-6">
        {/* Input search bar */}
      <input
        placeholder="Search note..."
        className="w-96 px-4 py-2 rounded-lg border border-purple-400 focus:outline-none focus:ring-2 focus:ring-purple-500"
      />
      {/* label to filter tasks */}
      <label htmlFor="Tasks" className="bg-purple-700 text-white px-4 py-2 rounded-lg">
        <select id="tasks" name="tasks">
          <option value="All">All</option>
          <option value="Complete">Complete</option>
          <option value="Incomplete">Incomplete</option>
        </select>
      </label>

      {/* Dark mode button */}
      <button className="bg-purple-700 text-white p-2 rounded-lg shadow-lg hover:scale-110 transition">🌙</button>
    </div>
  );
}

export default SearchBar;
